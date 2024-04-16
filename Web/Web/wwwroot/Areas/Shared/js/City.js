/*breadcrumb*/
var url = window.location.href.toLowerCase();

var breadcrumb = [];
breadcrumb.push({ title: "پنل ادمین", link: "/Admin/Dashboard" });
breadcrumb.push({ title: "شهر", link: "#" });




// عملیات مربوط به مدیریت شهر ها
var _area = "shared";
var _controller = "cities";
var entity = {

    // لیست شهر ها
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
                    [10, 25, 50],
                    [10, 25, 50]
                ],
                responsive: true,
                "ajax": {
                    "url": "/" + _area + "/" + _controller + "/Index",
                    "type": "POST",
                    "dataType": "json"
                },
                "columns": [
                    { "data": "id", "name": "شناسه" },
                    { "data": "name", "name": "نام شهر" },
                    { "data": "province", "name": "نام استان" },
                    {
                        data: "isEnabled",
                        render: function (data, type, row) {
                            var checked = '';
                            if (data) checked = 'checked';
                            return '<label class="toggle"><input onchange="entity.toggleEnable(' + row.id + ', this)" type="checkbox" ' + checked + '><span class="slider"></span><span class="labels" data-on="فعال" data-off="غیر فعال"></span></label>';
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
                entity.list.table.columns.adjust().draw();
            }, false);
        }
    },




    form: {

        initial: function () {
            $.material.init();
            $('.selectpicker').selectpicker('refresh');
        },



    },



    // افزودن شهر جدید
    create: {
        // لود کردن فرم افزودن شهر جدید
        loadForm: function () {
            $.get("/" + _area + "/" + _controller + "/Create",
                function (res) {
                    $("#base-modal").addClass('small-modal');
                    $("#modal-form").html(res);
                    entity.form.initial();
                    modal.open();

                    /*اعمال ولیدیشن به فرمی که با اجکس لود شده است*/
                    var form = $(".create-form")
                        .removeData("validator")
                        .removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                });
        },



        // ذخیره اطلاعات شهر جدید
        save: function (e) {
            e.preventDefault();
            var targetUrl = $(".create-form").attr("action");

            /*ولیدیت کردن فرم*/
            var $form = $(".create-form");
            $form.validate();
            if (!$form.valid()) return false;

            /*جمع آوری دیتای فرم*/
            var data = $(".create-form").serialize();

            /*ثبت فرم با اجکس*/
            $.post(targetUrl,
                data,
                function (res) {
                    if (res.isSuccess) {
                        modal.close();
                        entity.list.reload();
                    }
                    else {
                        $("#modal-form .create-error").html(res.message);
                        $("#base-modal").scrollTop(0);
                    }
                }).fail(function () {
                    $("#modal-form .create-error").html("ذخیره اطلاعات با خطا همراه بوده است. مجددا اقدام کنید.");
                    $("#base-modal").scrollTop(0);
                });
            return false;
        }
    },



    // ویرایش شهر
    edit: {
        // لود کردن فرم ویرایش شهر
        loadForm: function (id) {
            $.get("/" + _area + "/" + _controller + "/Edit/" + id,
                function (res) {
                    $("#base-modal").addClass('small-modal');
                    $("#modal-form").html(res);
                    entity.form.initial();
                    modal.open();

                    /*اعمال ولیدیشن به فرمی که با اجکس لود شده است*/
                    var form = $(".edit-form")
                        .removeData("validator")
                        .removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                });
        },


        // ذخیره اطلاعات ویرایش شده
        save: function (e) {
            e.preventDefault();
            var targetUrl = $(".edit-form").attr("action");

            /*ولیدیت کردن فرم*/
            var $form = $(".edit-form");
            $form.validate();
            if (!$form.valid()) return false;

            /*جمع آوری دیتای فرم*/
            var data = $(".edit-form").serialize();

            /*ثبت فرم با اجکس*/
            $.ajax({
                url: targetUrl,
                enctype: 'multipart/form-data',
                type: 'put',
                dataType: "json",
                data: data,
                success: function (res) {
                    if (res.isSuccess) {
                        modal.close();
                        entity.list.reload();
                    }
                    else {
                        $("#modal-form .edit-error").html(res.message);
                        $("#base-modal").scrollTop(0);
                    }
                }
            }).fail(function () {
                $("#modal-form .edit-error").html("ذخیره اطلاعات با خطا همراه بوده است. مجددا اقدام کنید.");
                $("#base-modal").scrollTop(0);
            });
            return false;
        },



    },




    // حذف شهر
    delete: {
        loadForm: function (id) {
            if (!id)
                showNotification("لطفا ابتدا شهر مورد نظر را انتخاب نمایید !");
            else
                swal({
                    title: 'آیا مطمئنید ؟',
                    text: "پس از حذف، اطلاعات قابل برگشت نیست.!",
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
                            text: 'شهر مورد نظر با موفقیت حذف شد',
                            type: 'success',
                            confirmButtonClass: "btn btn-success",
                            confirmButtonText: "باشه",
                            buttonsStyling: false
                        });
                    }
                    else {
                        swal({
                            title: 'حذف نشد !',
                            text: 'حذف شهر با خطا همراه بوده است. مجددا اقدام کنید!',
                            type: 'error',
                            confirmButtonClass: "btn btn-danger",
                            confirmButtonText: "باشه",
                            buttonsStyling: false
                        });
                    }
                }
            }).fail(function () {
                swal({
                    title: 'حذف نشد !',
                    text: 'حذف شهر با خطا همراه بوده است. مجددا اقدام کنید!',
                    type: 'error',
                    confirmButtonClass: "btn btn-danger",
                    confirmButtonText: "باشه",
                    buttonsStyling: false
                });
            });

        }

    },



    //تغییر وضعیت فعال بودن یا نبودن پست در صفحه ایندکس
    toggleEnable: function (id, el) {
        $.post("/" + _area + "/" + _controller + "/ToggleEnable/" + id, function (res) {
            if (!res.status) {
                showNotification(res.message, 'danger');

                if ($(el).prop('checked'))
                    $(el).prop('checked', false);
                else
                    $(el).prop('checked', true);
            }
        })
    },


}



entity.list.initial();
















