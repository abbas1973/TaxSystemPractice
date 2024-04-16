/*breadcrumb*/
var breadcrumb = [];
breadcrumb.push({ title: "پنل کاربری", link: "/AuthSystem/Dashboard" });
breadcrumb.push({ title: "نقش‌ ها", link: "#" });


/*BaseAddress*/
var _area = 'AuthSystem';
var _controller = 'Roles';
var _baseUrl = `/${_area}/${_controller}/`;




// عملیات مربوط به مدیریت نقش ها
var entity = {

    // لیست نقش ها
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
                    "dataType": "json"
                },
                "columns": [
                    { "data": "id", "name": "شناسه" },
                    { "data": "title", "name": "عنوان" },
                    { "data": "description", "name": "توضیحات" }, {
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
                                + "<a onclick='entity.delete.loadForm(" + data.id + ")' class='btn btn-simple btn-danger btn-icon' title='حذف' data-toggle='tooltip'><i class='material-icons'>close</i></a>";

                            return btns;
                        }
                    }
                ],
                "serverSide": "true",
                "order": [0, "desc"],
                "processing": "true",
                'columnDefs': [{
                    'targets': [4], /* column index */
                    'orderable': false, /* true or false */
                }]

            });
        },


        //رفرش کردن دیتا تیبل
        reload: function () {
            entity.list.table.ajax.reload(function () {
                //$.material.init();
            }, false);
        }
    },


    // فرم افزودن و ویرایش
    form: {
        //آماده سازی فرم ها
        initial: () => {
            $.material.init();
        },

        // اعتبار سنجی فرم ها
        validate: () => {
            var data = entity.form.collect();

            // عنوان
            if (!data.Title) {
                showNotification('عنوان نقش را وارد کنید!', 'danger')
                return null;
            }
            else if (data.Title.length > 200) {
                showNotification('عنوان نقش حداکثر 200 کاراکتر باشد!', 'danger')
                return null;
            }

            // توضیحات
            if (data.Description && data.Description.length > 500) {
                showNotification('توضیحات نقش حداکثر 500 کاراکتر باشد!', 'danger')
                return null;
            }

            // شناسه
            if ($('#Id').length > 0 && !data.Id) {
                showNotification('شناسه نقش را مشخص کنید!', 'danger')
                return null;
            }

            return data;

        },


        // جمع آوری دیتا از فرم ها
        collect: () => {
            var data = {
                __RequestVerificationToken: $('[name=__RequestVerificationToken]').val(),
                Title: $('#Title').val(),
                Description: $('#Description').val(),
                Claims: []
            }

            if ($('#Id').length > 0)
                data.Id = $('#Id').val();

            if ($('#IsEnabled').length > 0)
                data.IsEnabled = $('#IsEnabled').is(':checked');

            data.Claims = entity.form.GetClaims();
            return data;
        },



        // جمع آوری کلایم ها
        GetClaims: () => {
            var claims = [];
            var inps = $('.claim-section input:checked');
            for (var i = 0; i < inps.length; i++)
                claims.push($(inps[i]).val());
            return claims;
        },



        // تغییر وضعیت دسترسی ها
        toggleClaim: (inp) => {
            var level = parseInt($(inp).data('level'));
            var parent = $(inp).parents(`.claim-item[data-level=${level}]`).parent();


            if ($(inp).is(':checked')) {
                // اگر والد انتخاب شده بود، نمیتواند زیر مجموعه را جداگانه انتخاب کند
                var checkedParents = $(inp).parents(`.checkbox`).parent().parents('.claim-item').find(`> .checkbox input:checked`);
                if (checkedParents.length > 0) {
                    showNotification('برای انتخاب دسترسی به زیر مجموعه، ابتدا دسترسی والد را بردارید!', 'warning');
                    $(inp).prop('checked', false)
                    return;
                }


                // با انتخاب یک دسترسی، همه زیر مجموعه های آن هم شامل میشوند و از حالت انتخاب در می آیند
                $(inp).parents(`.claim-item[data-level=${level}]`)
                    .find(`> .claim-item input`).prop('checked', false)


                // اگر همه فرزندان مستقیم یک دسترسی انتخاب شده باشند،
                // خود آن دسترسی انتخاب میشود و فرزندانش از حالت انتخاب خارج میشوند.
                if ($(parent).hasClass('claim-item')) {
                    var childs = $(parent).find(`> .claim-item[data-level=${level}] input[data-level=${level}]`);
                    var notCheckedChilds = $(parent).find(`> .claim-item[data-level=${level}] input[data-level=${level}]:not(:checked)`);
                    if (notCheckedChilds.length == 0) {
                        for (var i = 0; i < childs.length; i++) {
                            $(childs[i]).prop('checked', false);
                        }
                        $(parent).find(`> .checkbox input`).prop('checked', true);
                    }
                }
            }
            else {
                // اگر دسته بندی از حالت انتخاب خارج شود، والد آن هم از حالت انتخاب خارج میشود
                $(parent).find(`> .checkbox input`).prop('checked', false);
            }

            $.material.init();
        },


        // باز و بسته کردن دسترسی ها
        toggleCollapse: (el) => {
            var claimItem = $(el).closest('.claim-item');

            if ($(el).hasClass('open')) {
                $(el).removeClass('open').text('+');
                $(claimItem).removeClass('open');
                $(claimItem).find(' > .claim-item').slideUp();
            }
            else {
                $(el).addClass('open').text('-');
                $(claimItem).addClass('open');
                $(claimItem).find(' > .claim-item').slideDown();
            }
        },


        // باز کردن دسترسی های انتخاب شده در فرم ویرایش در حالت درختی
        initialToggleCollapse: () => {
            var checked = $('input[type=checkbox].claim:checked');

            checked.each((index, item) => {
                var parents = $(item).parents('.claim-item');
                for (var i = 1; i < parents.length; i++) {
                    if (!$(parents[i]).hasClass('open')) {
                        var collapse = $(parents[i]).find('> .checkbox > span.collapse');
                        entity.form.toggleCollapse(collapse);
                    }
                }
            })
        }

    },



    // افزودن نقش جدید
    create: {
        // لود کردن فرم افزودن نقش جدید
        loadForm: function () {
            $.get(_baseUrl + "Create",
                function (res) {
                    $("#modal-form").html(res);
                    entity.form.initial();
                    modal.open();
                });
        },



        // ذخیره اطلاعات نقش جدید
        save: function (e) {
            e.preventDefault();
            var targetUrl = `${_baseUrl}Create`;

            var data = entity.form.validate();
            if (!data)
                return false;

            $.post(targetUrl,
                data,
                function (res) {
                    if (res.isSuccess) {
                        entity.list.reload();
                        modal.close();
                    }
                    else {
                        showNotification(res.message, 'danger');
                    }
                });
            return false;
        }
    },



    // ویرایش نقش
    edit: {
        // لود کردن فرم ویرایش نقش
        loadForm: function (id) {
            $.get(_baseUrl + "Edit/" + id,
                function (res) {
                    $("#modal-form").html(res);
                    entity.form.initial();
                    entity.form.initialToggleCollapse();
                    modal.open();
                });
        },


        // ذخیره اطلاعات ویرایش شده
        save: function (e) {
            e.preventDefault();
            var targetUrl = `${_baseUrl}Edit`
            
            var data = entity.form.validate();
            if (!data)
                return false;

            $.put(targetUrl,
                data,
                function (res) {
                    if (res.isSuccess) {
                        entity.list.reload();
                        modal.close();
                    }
                    else {
                        showNotification(res.message, 'danger');
                    }
                });
            return false;
        },
    },




    // حذف نقش
    delete: {
        loadForm: function (id) {
            if (!id)
                showNotification("لطفا ابتدا نقش مورد نظر را انتخاب نمایید !");
            else
                swal({
                    title: 'آیا مطمئنید ؟',
                    text: "تمامی دسترسی های این نقش حذف خواهد شد!",
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
            $.delete(_baseUrl + "Delete/" + id,
                function (res) {
                    if (res.isSuccess) {
                        entity.list.reload();
                        swal({
                            title: 'حذف شد !',
                            text: 'نقش مورد نظر با موفقیت حذف شد',
                            type: 'success',
                            confirmButtonClass: "btn btn-success",
                            confirmButtonText: "باشه",
                            buttonsStyling: false
                        });
                    }
                    else {
                        swal({
                            title: 'حذف نشد !',
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


    //تغییر وضعیت فعال بودن یا نبودن نقش در صفحه ایندکس
    toggleEnable: function (el, id) {
        $.post(_baseUrl + 'ToggleEnable/' + id, function (res) {
            if (res.isSuccess)
                return;

            showNotification('تغییر وضعیت کاربر با خطا همراه بوده است!', 'danger');
            if ($(el).prop('checked'))
                $(el).prop('checked', false);
            else
                $(el).prop('checked', true);
        });
    }

}



entity.list.initial();













