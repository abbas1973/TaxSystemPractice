/*breadcrumb*/
var breadcrumb = [];
breadcrumb.push({ title: "پنل ادمین", link: "/Admin/Dashboard" });
breadcrumb.push({ title: "مدیریت کاربران", link: "#" });


/*BaseAddress*/
var _area = 'AuthSystem';
var _controller = 'Users';
var _baseUrl = `/${_area}/${_controller}/`;


var entity = {

    // لیست کاربر ها
    list: {
        // آبجکت دیتاتیبل
        table: null,


        // راه اندازی دیتاتیبل
        initial: function () {
            this.table = $('#datatables').DataTable({
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip"]').tooltip();
                },
                language: {
                    url: "/assets/datatables/fa-lang.json"
                },
                "pagingType": "full_numbers",
                "lengthMenu": [
                    [10, 25, 50, -1],
                    [10, 25, 50, "All"]
                ],
                responsive: true,
                "ajax": {
                    "url": _baseUrl + "Index",
                    "type": "POST",
                    "dataType": "json",
                    "data": function (d) {
                        return $.extend({}, d, filter.collect());
                    },
                },
                "columns": [
                    { "data": "id", "name": "شناسه" },
                    { "data": "name", "name": "نام کامل" },
                    { "data": "mobile", "name": "تلفن" },
                    { "data": "username", "name": "نام کاربری" },
                    { "data": "city", "name": "شهر" },
                    {
                        data: "isEnabled",
                        render: function (data, type, row) {
                            var checked = '';
                            if (data) checked = 'checked';
                            return '<label class="toggle for-isEnabled"><input onchange="entity.toggleEnable(this, ' + row.id + ')" type="checkbox" ' + checked + '><span class="slider"></span><span class="labels" data-on="فعال" data-off="غیرفعال"></span></label>';
                        }
                    },
                    {
                        data: null,
                        className: "text-left",
                        render: function (data, type, row) {
                            var btns = "<a onclick='entity.edit.loadForm(" + data.id + ")' class='btn btn-simple btn-info btn-icon' title='ویرایش' data-toggle='tooltip'><i class='material-icons'>edit</i></a>"
                                + "<a onclick='entity.changePassword.loadForm(" + data.id + ")' class='btn btn-simple btn-primary btn-icon' title='تغییر کلمه عبور' data-toggle='tooltip'><i class='material-icons'>lock</i></a>"
                                //+ "<a onclick='entity.resetPassword.loadForm(" + data.id + ")' class='btn btn-simple btn-primary btn-icon' title='ریست کردن کلمه عبور' data-toggle='tooltip'><i class='material-icons'>lock</i></a>"
                                + "<a onclick='entity.delete.loadForm(" + data.id + ")' class='btn btn-simple btn-danger btn-icon' title='حذف' data-toggle='tooltip'><i class='material-icons'>close</i></a>";

                            return btns;
                        }
                    }
                ],
                "serverSide": "true",
                "order": [0, "desc"],
                "processing": "true",
                'columnDefs': [{
                    'targets': [1,2,3,4,5,6], 
                    'orderable': false,
                }]

            });
        },


        //رفرش کردن دیتا تیبل
        reload: function (resetPage) {
            if (resetPage != true)
                resetPage = false;

            entity.list.table.ajax.reload(function () {
                entity.list.table.columns.adjust().draw();
            }, resetPage);
        }
    },


    // آماده سازی فرم ها
    form: {
        initial: function () {
            $('#base-modal').removeClass('small-modal');
            $('.selectpicker').selectpicker('refresh');
            $.material.init();
            if ($('#OldCityId').length > 0) {
                var provinceId = $('#ProvinceId option:selected').val();
                getCities(provinceId);
            }
        },


        // جمع آوری داده از فرم
        collectData: function () {
            
            var formData = new FormData();

            if ($('#Id').length > 0)
                formData.append('Id', $('#Id').val());

            formData.append('FirstName', $('#FirstName').val());
            formData.append('LastName', $('#LastName').val());
            formData.append('Mobile', $('#Mobile').val());
            formData.append('Username', $('#Username').val());
            formData.append('Password', $('#Password').val());
            formData.append('RePassword', $('#RePassword').val());
            formData.append('CityId', $('#CityId option:selected').val());

            var roles = $('.role-item');
            for (var i = 0; i < roles.length; i++) {
                if ($(roles[i]).is(':checked'))
                    formData.append('RoleIds[]', $(roles[i]).val());
            }
            formData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

            if ($('#IsEnabled').length > 0)
                formData.append('IsEnabled', $('#IsEnabled').is(':checked'));           

            return formData;
        }

    },


    // افزودن پرسنل جدید
    create: {
        // لود کردن فرم افزودن پرسنل جدید
        loadForm: function () {
            $.get("/" + _area + "/" + _controller + "/Create",
                function (res) {
                    $("#modal-form").html(res);
                    entity.form.initial();
                    modal.open();
                });
        },



        // ذخیره اطلاعات پرسنل جدید
        save: function (e) {
            e.preventDefault();
            var targetUrl = `${_baseUrl}Create`;

            /*ولیدیت کردن فرم*/
            if (!Validate()) {
                return false;
            }

            /*جمع آوری دیتای فرم*/
            var data = entity.form.collectData();

            /*ثبت فرم با اجکس*/
            $.ajax({
                url: targetUrl,
                enctype: 'multipart/form-data',
                processData: false,
                contentType: false,
                dataType: "json",
                type: 'POST',
                data: data,
                success: function (res) {
                    if (res.isSuccess) {
                        entity.list.reload();
                        modal.close();
                    }
                    else {
                        showNotification(res.message, 'danger');
                        $("#modal-form .create-error").html(res.message);
                        $("#base-modal").scrollTop(0);
                    }
                }
            });
            return false;
            
        }
    },


    // ویرایش پرسنل
    edit: {
        // لود کردن فرم ویرایش پرسنل
        loadForm: function (id) {
            $.get("/" + _area + "/" + _controller + "/Edit/" + id,
                function (res) {
                    $("#modal-form").html(res);
                    entity.form.initial();
                    modal.open();
                });
        },



        // ذخیره اطلاعات ویرایش شده
        save: function () {

            var targetUrl = $(".edit-form").attr("action");

            /*ولیدیت کردن فرم*/
            if (!Validate()) {
                return false;
            }

            /*جمع آوری دیتای فرم*/
            var data = entity.form.collectData();

            /*ثبت فرم با اجکس*/
            $.ajax({
                url: targetUrl,
                enctype: 'multipart/form-data',
                processData: false,
                contentType: false,
                type: 'put',
                data: data,
                success: function (res) {
                    if (res.isSuccess) {
                        entity.list.reload();
                        modal.close();
                    }
                    else {
                        showNotification(res.message, 'danger');
                    }
                }
            });
            return false;
        },

    },



    // ریست کردن کلمه عبور پرسنل
    resetPassword: {
        loadForm: function (id) {
            if (!id)
                showNotification("لطفا ابتدا پرسنل مورد نظر را انتخاب نمایید !");
            else
                swal({
                    title: 'آیا مطمئنید ؟',
                    text: "کلمه عبور کاربر ریست می شود و کلمه عبور جدید به تلفن همراه ثبت شده برای کاربر پیامک می گردد.",
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonClass: 'btn btn-danger',
                    cancelButtonClass: 'btn btn-default',
                    confirmButtonText: 'بله ، ریست شود !',
                    cancelButtonText: 'لغو',
                    buttonsStyling: false
                }).then(function (isConfirm) {
                    if (isConfirm)
                        entity.resetPassword.confirm(id);
                });
        },

        confirm: function (id) {
            $.post("/" + _area + "/" + _controller + "/resetPassword/" + id,
                function (res) {
                    if (res.status) {
                        entity.list.reload();
                        swal({
                            title: 'ریست شد !',
                            text: 'کلمه عبور پرسنل مورد نظر با موفقیت ریست شد.',
                            type: 'success',
                            confirmButtonClass: "btn btn-success",
                            confirmButtonText: "باشه",
                            buttonsStyling: false
                        });
                    }
                    else {
                        swal({
                            title: 'ریست نشد !',
                            text: res.message,
                            type: 'error',
                            confirmButtonClass: "btn btn-danger",
                            confirmButtonText: "باشه",
                            buttonsStyling: false
                        });
                    }
                });

        }
    },




    // تغییر کلمه عبور پرسنل
    changePassword: {
        // لود کردن فرم تغییر کلمه عبور
        loadForm: function (id, fullName) {
            $.get("/" + _area + "/" + _controller + "/ChangePassword/" + id,
                { FullName: fullName},
                function (res) {
                    $("#modal-form").html(res);
                    $('#base-modal').addClass('small-modal');
                    modal.open();

                    /*اعمال ولیدیشن به فرمی که با اجکس لود شده است*/
                    var form = $(".change-pass-form")
                        .removeData("validator")
                        .removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                });
        },



        // ذخیره کلمه عبور جدید
        save: function (e) {
            e.preventDefault();
            var id = $('#Id').val();
            var targetUrl = `/${_area}/${_controller}/ChangePassword/${id}`

            var data = {
                Password: $('#Password').val(),
                RePassword: $('#RePassword').val()
            }
            if (!data.Password || !data.RePassword) {
                showNotification('کلمه عبور و تکرار آن الزامیست!', 'danger');
                return false;
            }

            /*ثبت فرم با اجکس*/
            $.post("/" + _area + targetUrl,
                data,
                function (res) {
                    if (res.isSuccess) {
                        modal.close();
                        swal({
                            title: 'تغییر یافت.',
                            text: 'کلمه عبور با موفقیت تغییر یافت.',
                            type: 'success',
                            confirmButtonClass: "btn btn-success",
                            confirmButtonText: "باشه",
                            buttonsStyling: false
                        });
                    }
                    else {
                        $('.change-pass-form .error').html(res.message);
                    }
                });
            return false;
        }
    },


    // حذف پرسنل
    delete: {
        loadForm: function (id) {
            if (!id)
                showNotification("لطفا ابتدا پرسنل مورد نظر را انتخاب نمایید !");
            else
                swal({
                    title: 'آیا مطمئنید ؟',
                    text: "بعد از حذف، اطلاعات پرسنل قابل برگشت نیست!",
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonClass: 'btn btn-danger',
                    cancelButtonClass: 'btn btn-default',
                    confirmButtonText: 'بله ، حذف شود !',
                    cancelButtonText: 'لغو',
                    buttonsStyling: false
                }).then(function (isConfirm) {
                    if (isConfirm)
                        entity.delete.confirm(id);
                });
        },

        confirm: function (id) {
            $.ajax({
                url: "/" + _area + "/" + _controller + "/Delete/" + id,
                enctype: 'multipart/form-data',
                type: 'delete',
                dataType: "json",
                success: function (res) {
                    if (res.isSuccess) {
                        entity.list.reload();
                        swal({
                            title: 'حذف شد !',
                            text: 'پرسنل مورد نظر با موفقیت حذف شد',
                            type: 'success',
                            confirmButtonClass: "btn btn-success",
                            confirmButtonText: "باشه",
                            buttonsStyling: false
                        });
                    }
                    else {
                        swal({
                            title: 'حذف نشد !',
                            text: 'حذف پرسنل با خطا همراه بوده است. مجددا اقدام کنید!',
                            type: 'error',
                            confirmButtonClass: "btn btn-danger",
                            confirmButtonText: "باشه",
                            buttonsStyling: false
                        });
                    }
                }
            });
            

        }

    },


    //تغییر وضعیت فعال بودن یا نبودن کاربر در صفحه ایندکس
    toggleEnable: function (el, id) {
        $.post("/" + _area + "/" + _controller + "/ToggleEnable/" + id, function (res) {
            if (res.isSuccess)
                return;

            showNotification('تغییر وضعیت کاربر با خطا همراه بوده است!', 'danger');
            if ($(el).prop('checked'))
                $(el).prop('checked', false);
            else
                $(el).prop('checked', true);
        })
    }

}


entity.list.initial();
//filter.initial();


function getCities(id) {
    if (!id) {
        $('#CityId option').remove();
        return;
    }

    var data = {
        ProvinceId: id,
        JustEnabled: true,
    }

    $.get("/Shared/Cities/GetSelectList", data,
        function (res) {
            $('#CityId option').remove();
            var oldCityId = $('#OldCityId').val();
            $.each(res, function (i, item) {
                var selected = item.id == oldCityId ? "selected" : "";
                $('#CityId').append(`<option ${selected} value="${item.id}">${item.title}</option>`);
            });
            $('#CityId').selectpicker('refresh');
        });

}


var Validate = function () {

    var mobile = $('#Mobile').val();
    var username = $('#Username').val();
    var firstName = $('#FirstName').val();
    var lastName = $('#LastName').val();
    if (!username || !mobile || !firstName || !lastName) {
        $('.error').html('فیلدهای اجباری را کامل نمایید!');
        return false;
    }

    if ($('#Password').length > 0) {
        var password = $('#Password').val();
        var rePassword = $('#RePassword').val();

        if (!password || !rePassword)
            $('.error').html('فیلدهای اجباری را کامل نمایید!');

        if (password != rePassword) {
            $('.error').html('کلمه عبورهای وارد شده با هم مطابقت ندارند!');
            return false;
        }

        var regPass = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,50}$/;
        if (!regPass.test(password)) {
            $('.error').html('کلمه عبور را مطابق الگو وارد کنید!');
            return false;
        }
    }

    var regMobile = /^0\d{10}/;
    if (!regMobile.test(mobile)) {
        $('.error').html('شماره موبایل را بدرستی وارد کنید!');
        return false;
    }


    return true;
}


var changePasswordValidate = function () {
    var currentPassword = $('#CurrentPassword').val();
    var password = $('#Password').val();
    var rePassword = $('#RePassword').val();

    if (!currentPassword || !password || !rePassword) {
        $('.error').html('فیلدهای اطلاعاتی را کامل نمایید!');
        return false;
    }

    if (password != rePassword) {
        $('.error').html('کلمه عبورهای وارد شده با هم مطابقت ندارند!');
        return false;
    }

    var regPass = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,50}$/;
    if (!regPass.test(password)) {
        $('.error').html('کلمه عبور را مطابق الگو وارد کنید!');
        return false;
    }

    return true;
}


//==============================================
// فیلتر جستجو درخواست ها
//==============================================
var filter = {
    initial: _ => {
        filter.startDate.initial();
        filter.endDate.initial();
        filter.role.initial();
        filter.store.initial();

        $('.selectpicker').selectpicker('refresh');
    },



    //جمع آوری داده فیلتر
    collect: _ => {
        var data = {
            StartDate: $("input[name=FilterStartDate]").val(),
            EndDate: $("input[name=FilterEndDate]").val(),
            Name: $("#FilterName").val(),
            Username: $("#FilterUsername").val(),
            Mobile: $("#FilterMobile").val(),
            RoleId: $("#FilterRoleId").val(),
            StoreId: $("#FilterStoreId").val(),
            IsEnabled: $("#FilterIsEnabled").val(),
        }

        return data;
    },


    // فروشگاه
    store: {

        container: ".store",

        // راه اندازی جستجو فروشگاه
        initial: function () {
            var con = this.container;
            $("#FilterStoreId").selectize({
                create: true,
                valueField: 'id',
                labelField: 'title',
                searchField: ['title'],
                create: false,
                options: [],
                plugins: ["remove_button"],
                closeAfterSelect: true,
                render: {
                    item: function (item, escape) {
                        return '<div class="selected-product-selectize">' + item.title + '</div>';
                    },
                    option: function (item, escape) {
                        $(con).find('.selectize-dropdown .no-result').remove();
                        var tmp = "<div class='col-md-12'>"
                            + "<div class='col-md-12' style='padding:0'>"
                            + "<div>" + escape(item.title) + "</div>"
                            + "</div>";
                        return tmp;
                    }
                },
                onType: function (text) {
                    $(con).find('.selectize-dropdown .no-result').remove();
                    if (!this.currentResults.items.length)
                        $(con).find('.selectize-dropdown').css('display', 'block').append('<div class="no-result">موردی یافت نشد!</div>');
                },
                load: function (query, callback) {
                    if (!query.length) return callback();
                    $.ajax({
                        url: '/Global/stores/Search',
                        type: 'Post',
                        dataType: 'json',
                        data: {
                            word: query,
                        },
                        error: function () {
                            callback();
                        },
                        success: function (res) {
                            if (res && res.length > 0)
                                $(con).find('.selectize-dropdown .no-result').remove();
                            else
                                $(con).find('.selectize-dropdown').css('display', 'block').append('<div class="no-result">موردی یافت نشد!</div>');

                            callback(res);
                        },
                        beforeSend: function () {
                            $(con).find('.selectize-dropdown .no-result').remove();
                            $(".sender .selectize-control").addClass('loading');
                        },
                        complete: function () {
                            $(".sender .selectize-control").removeClass('loading');
                        }
                    });
                }
            })
            $(con).find(".selectize-control").addClass("form-group is-empty");
            $(con).find(".selectize-input").addClass("form-control text-box");
        }
    },


    // تاریخ شروع
    startDate: {

        // آبجکت vue
        obj: null,

        //راه اندازی تقویم تاریخ شروع
        initial: function () {
            if ($('#VueStartDate').length == 0)
                return;
            var sdate = ''; //moment()/*.subtract(7, 'd')*/.format('jYYYY/jMM/jDD');
            this.obj = new Vue({
                el: '#VueStartDate',
                data: {
                    date: sdate
                },
                components: {
                    DatePicker: VuePersianDatetimePicker
                },
                methods: {
                    onClose: function (x) {
                        filter.checkDates(filter.startDate, filter.endDate);
                    }
                }
            });
        }
    },



    // تاریخ پایان
    endDate: {
        // آبجکت vue
        obj: null,

        //راه اندازی تقویم تاریخ پایان
        initial: function () {
            if ($('#VueEndDate').length == 0)
                return;
            var edate = ''; // moment().format('jYYYY/jMM/jDD');
            this.obj = new Vue({
                el: '#VueEndDate',
                data: {
                    date: edate
                },
                components: {
                    DatePicker: VuePersianDatetimePicker
                },
                methods: {
                    onClose: function (x) {
                        filter.checkDates(filter.startDate, filter.endDate);
                    }
                }
            });
        }
    },



    // بررسی تاریخ شروع و پایان
    checkDates: function (start, end) {
        var sdate = start.obj.date;
        var edate = end.obj.date;
        if (sdate && edate && sdate > edate) { 
            showNotification("تاریخ شروع نمیتواند بعد از تاریخ پایان باشد!", 'danger');
            $("input[name=FilterStartDate]").val('').prev('input').val('');
            $("input[name=FilterEndDate]").val('').prev('input').val('');
        }
    },



    //نقش ها
    role: {
        initial: _ => {
            if ($('#FilterRoleId').length == 0)
                return;

            var opt = "<option value='' selected>همه</option>"
            $('#FilterRoleId').prepend(opt);
        }
    },



    // خالی کردن سلکتایز از ایتم انتخاب شده
    clearSelectize: function (el) {
        var $select = $(el).selectize();
        var control = $select[0].selectize;
        control.clear();
        control.renderCache = {};
        control.clearOptions();
        control.refreshOptions(true);
    },



}



