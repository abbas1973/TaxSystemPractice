/*breadcrumb*/
var breadcrumb = [];
breadcrumb.push({ title: "پنل کاربری", link: "/AuthSystem/Dashboard" });
breadcrumb.push({ title: "صورتحساب‌ ها", link: "#" });


/*BaseAddress*/
var _area = 'TaxSystem';
var _controller = 'Invoices';
var _baseUrl = `/${_area}/${_controller}/`;



//==============================================
// فیلتر جستجو صورتحساب ها
//==============================================
var filter = {

    initial: _ => {
        $('.selectpicker').selectpicker('refresh');
        filter.sendDateFrom.initial();
        filter.sendDateTo.initial();
        filter.invoiceDateFrom.initial();
        filter.invoiceDateTo.initial();
    },


    //جمع آوری داده فیلتر
    collect: _ => {
        var data = {
            InvoiceDateFrom: $("input[name=FilterInvoiceDateFrom]").val(),
            InvoiceDateTo: $("input[name=FilterInvoiceDateTo]").val(),
            InvoiceNumber: $("#FilterInvoiceNumber").val(),
            TaxId: $("#FilterTaxId").val(),
            BuyerName: $("#FilterName").val(),
            BuyerNationalCode: $("#FilterNationalCode").val(),
            SendDateFrom: $("input[name=FilterSendDateFrom]").val(),
            SendDateTo: $("input[name=FilterSendDateTo]").val(),
            PayType: $("#FilterPayType").val(),
            TaxInvoiceType: $("#FilterTaxInvoiceType").val(),
            TaxInvoicePattern: $("#FilterTaxInvoicePattern").val(),
            TaxInvoiceSubject: $("#FilterTaxInvoiceSubject").val(),
            SendStatus: $("#FilterSendStatus").val()
        }

        return data;
    },


    // تاریخ شروع صورتحساب
    invoiceDateFrom: {

        // آبجکت vue
        obj: null,

        //راه اندازی تقویم تاریخ شروع
        initial: function () {
            if ($('#VueInvoiceDateFrom').length == 0)
                return;
            var sdate = '';// moment().subtract(1, 'd').format('jYYYY/jMM/jDD');
            this.obj = new Vue({
                el: '#VueInvoiceDateFrom',
                data: {
                    date: sdate
                },
                components: {
                    DatePicker: VuePersianDatetimePicker
                },
                methods: {
                    onClose: function (x) {
                        //filter.checkDates(filter.createStartDate, filter.createEndDate);
                    }
                }
            });
        }
    },


    // تاریخ پایان صورتحساب
    invoiceDateTo: {
        // آبجکت vue
        obj: null,

        //راه اندازی تقویم تاریخ پایان
        initial: function () {
            if ($('#VueInvoiceDateTo').length == 0)
                return;
            var edate = ''; // moment().format('jYYYY/jMM/jDD');
            this.obj = new Vue({
                el: '#VueInvoiceDateTo',
                data: {
                    date: edate
                },
                components: {
                    DatePicker: VuePersianDatetimePicker
                },
                methods: {
                    onClose: function (x) {
                        //filter.checkDates(filter.createEndDate, filter.createEndDate);
                    }
                }
            });
        }
    },




    // تاریخ شروع ارسال صورتحساب
    sendDateFrom: {

        // آبجکت vue
        obj: null,

        //راه اندازی تقویم تاریخ شروع
        initial: function () {
            if ($('#VueSendDateFrom').length == 0)
                return;
            var sdate = '';// moment().subtract(1, 'd').format('jYYYY/jMM/jDD');
            this.obj = new Vue({
                el: '#VueSendDateFrom',
                data: {
                    date: sdate
                },
                components: {
                    DatePicker: VuePersianDatetimePicker
                },
                methods: {
                    onClose: function (x) {
                        //filter.checkDates(filter.createStartDate, filter.createEndDate);
                    }
                }
            });
        }
    },


    // تاریخ پایان ارسال صورتحساب
    sendDateTo: {
        // آبجکت vue
        obj: null,

        //راه اندازی تقویم تاریخ پایان
        initial: function () {
            if ($('#VueSendDateTo').length == 0)
                return;
            var edate = ''; // moment().format('jYYYY/jMM/jDD');
            this.obj = new Vue({
                el: '#VueSendDateTo',
                data: {
                    date: edate
                },
                components: {
                    DatePicker: VuePersianDatetimePicker
                },
                methods: {
                    onClose: function (x) {
                        //filter.checkDates(filter.createEndDate, filter.createEndDate);
                    }
                }
            });
        }
    },
}



// عملیات مربوط به مدیریت صورتحساب ها
var entity = {

    // لیست صورتحساب ها
    list: {
        // آبجکت دیتاتیبل
        table: null,

        //صورتحساب های انتخاب شده
        selectedItems: [],

        // راه اندازی دیتاتیبل
        initial: function () {
            this.table = $('#datatables').DataTable({
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip"]').tooltip();
                    $.material.init();

                    var items = $('td.price-format');
                    for (var i = 0; i < items.length; i++) {
                        var price = $(items[i]).html();
                        $(items[i]).html(getPriceFormat(price));
                    }
                },
                language: {
                    url: "/assets/datatables/fa-lang.json"
                },
                "pagingType": "full_numbers",
                "lengthMenu": [
                    [10, 25, 50, -1],
                    [10, 25, 50, "All"]
                ],
                responsive: false,
                "ajax": {
                    "url": _baseUrl + "Index",
                    "type": "POST",
                    "dataType": "json",
                    "data": function (d) {
                        return $.extend({}, d, filter.collect());
                    },
                },
                "columns": [
                    {
                        data: null,
                        name: "چکباکس",
                        render: function (data, type, row) {
                            var checked = "";
                            if (entity.list.selectedItems.includes(row.id))
                                checked = "checked";
                            return `<div class="checkbox form-horizontal-checkbox stop-propagate"><label><input type="checkbox" rel="${row.id}" onchange="wntity.list.selectInvoice(event, this,${row.id})" ${checked}></label></div>`;
                        }
                    },
                    { "data": "id", "name": "شناسه" },
                    { "data": "invoiceNumber", "name": "شماره صورتحساب" },
                    {
                        data: "invoiceDate",
                        render: function (data, type, row) {
                            return row.invoiceDateFa
                        }
                    },
                    { "data": "buyerName" },
                    { "data": "nationalCode" },
                    { "data": "totalCount" },
                    { "data": "totalAmount", className:"price-format" },
                    {
                        data: "payType",
                        render: function (data, type, row) {
                            return row.payTypeFa
                        }
                    },
                    {
                        data: "taxInvoiceType",
                        render: function (data, type, row) {
                            return row.taxInvoiceTypeFa
                        }
                    },
                    {
                        data: "taxInvoicePattern",
                        render: function (data, type, row) {
                            return row.taxInvoicePatternFa
                        }
                    },
                    {
                        data: "taxInvoiceSubject",
                        render: function (data, type, row) {
                            return row.taxInvoiceSubjectFa
                        }
                    },
                    {
                        data: "sendStatus",
                        render: function (data, type, row) {
                            var st = `<div>${row.sendStatusFa}</div>`;

                            if(row.isSendable)
                                st += `<button type="button" class="btn btn-xs btn-success" onclick="entity.sendToTax(${row.id})" data-toggle="tooltip" title="ارسال به سامانه مودیان مالیاتی">ارسال صورتحساب</button>`;
                                
                            if (row.isInquiryable)
                                st += `<button type="button" class="btn btn-xs btn-success" onclick="entity.inquiry(${row.id})" data-toggle="tooltip" title="استعلام وضعیت از سامانه مودیان مالیاتی">استعلام صورتحساب</button>`;

                            return st;
                        }
                    },
                    {
                        data: "sendDate",
                        render: function (data, type, row) {
                            return row.sendDateFa
                        }
                    },
                    { "data": "serialNumber" },
                    { "data": "taxId" },
                    { "data": "taxStatus" },
                    {
                        data: null,
                        className: "text-left",
                        render: function (data, type, row) {
                            var btns = "";
                            //btns += "<a onclick='entity.edit.loadForm(" + data.id + ")' class='btn btn-simple btn-info btn-icon' title='ویرایش' data-toggle='tooltip'><i class='material-icons'>edit</i></a>";
                            btns += "<a onclick='entity.details(" + data.id + ")' class='btn btn-simple btn-primary btn-icon' title='جزییات' data-toggle='tooltip'><i class='material-icons'>dvr</i></a>";

                            if(row.isDeleteable)
                                btns += "<a onclick='entity.delete.loadForm(" + data.id + ")' class='btn btn-simple btn-danger btn-icon' title='حذف' data-toggle='tooltip'><i class='material-icons'>close</i></a>";

                            return btns;
                        }
                    }
                ],
                "serverSide": "true",
                "order": [3, "desc"],
                "processing": "true",
                'columnDefs': [{
                    'targets': [0],
                    'orderable': false,
                }]

            });
        },


        //رفرش کردن دیتا تیبل
        reload: function () {
            entity.list.table.ajax.reload(function () {
                //$.material.init();
            }, false);
        },


        // انتخاب درخواست ها از طریق چکباکس
        selectInvoice: function (e, el, requestId) {
            e = e || window.event;
            e.preventDefault();
            e.stopPropagation();

            if ($(el).is(":checked"))
                entity.list.selectedItems.push(requestId);
            else {
                entity.list.selectedItems = entity.list.selectedItems.filter(function (id) {
                    return id != requestId;
                });
                $('.select-all').prop('checked', false);
            }
        },



        // انتخاب همه درخواست های صفحه
        checkAll: function (e, el) {
            e = e || window.event;
            e.preventDefault();
            e.stopPropagation();

            var tds = $('#datatables').find('tbody input[type=checkbox]');
            for (var i = 0; i < tds.length; i++) {
                var id = $(tds[i]).attr("rel");
                if ($(el).is(":checked") && !$(tds[i]).is(":checked")) {
                    entity.list.selectedItems.push(id);
                    $(tds[i]).prop('checked', true);
                }
                else if (!$(el).is(":checked") && $(tds[i]).is(":checked")) {
                    entity.list.selectedItems = entity.list.selectedItems.filter(function (id) {
                        return id != id;
                    });
                    $(tds[i]).prop('checked', false);
                }
            }
            $.material.init();
        },

    },


    // جزییات سفارش
    details: (id) => {
        $.get(_baseUrl + "GetById/" + id,
            function (res) {
                $("#modal-form").html(res);
                var element = $(".json");
                var txt = element.text();
                if (txt && txt.length > 0) {
                    var obj = JSON.parse(element.text());
                    element.html(JSON.stringify(obj, undefined, 2));
                }
                modal.open();
            });
    },

    // گرفتن خروجی اکسل
    export: function () {

        var rand = Math.floor(Math.random() * 10000000);
        var data = filter.collect();
        var queryString = $.param(data);

        var url = _baseUrl + "Export";
        if (queryString.length > 0)
            url += "?" + queryString;
        window.open(url);
    },


    // ارسال
    sendToTax: (id) => {
        var ids = [];
        if (id)
            ids.push(id);
        else
            ids = entity.list.selectedItems;

        if (!ids || ids.length == 0) {
            showNotification('صورتحساب مورد نظر را انتخاب کنید!', 'danger');
            return;
        }

        $.post(_baseUrl + "sendToTax",
            {
                InvoiceIds: ids
            },
            function (res) {
                if (res.isSuccess == false)
                    showNotification(res.message, 'danger');
                else {
                    showNotification('صورتحساب ها با موقیت به سامانه مالیاتی ارسال شدند. وضعیت صورتحساب های ارسال شده بصورت خودکار تا 20 ثانیه دیگر از سامانه مالیاتی استعلام گرفته میشود.', 'success');
                    entity.list.reload();
                }
            });
    },


    // استعلام
    inquiry: (id) => {
        var ids = [];
        if (id)
            ids.push(id);
        else
            ids = entity.list.selectedItems;

        if (!ids || ids.length == 0) {
            showNotification('صورتحساب مورد نظر را انتخاب کنید!', 'danger');
            return;
        }

        $.post(_baseUrl + "inquiry",
            {
                InvoiceIds: ids
            },
            function (res) {
                if (res.isSuccess == false)
                    showNotification(res.message, 'danger');
                else {
                    entity.list.reload();
                    showNotification('استعلام صورتحساب ها با موفقیت انجام شد. نتیجه استعلام را میتوانید در جزییات صورتحساب مشاهده کنید.', 'success');
                }
            });
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
                showNotification('عنوان صورتحساب را وارد کنید!', 'danger')
                return null;
            }
            else if (data.Title.length > 200) {
                showNotification('عنوان صورتحساب حداکثر 200 کاراکتر باشد!', 'danger')
                return null;
            }

            // توضیحات
            if (data.Description && data.Description.length > 500) {
                showNotification('توضیحات صورتحساب حداکثر 500 کاراکتر باشد!', 'danger')
                return null;
            }

            // شناسه
            if ($('#Id').length > 0 && !data.Id) {
                showNotification('شناسه صورتحساب را مشخص کنید!', 'danger')
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

        
    },



    // افزودن صورتحساب جدید
    create: {
        // لود کردن فرم افزودن صورتحساب جدید
        loadForm: function () {
            $.get(_baseUrl + "Create",
                function (res) {
                    $("#modal-form").html(res);
                    entity.form.initial();
                    modal.open();
                });
        },



        // ذخیره اطلاعات صورتحساب جدید
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



    // ویرایش صورتحساب
    edit: {
        // لود کردن فرم ویرایش صورتحساب
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




    // حذف صورتحساب
    delete: {
        loadForm: function (id) {
            if (!id)
                showNotification("لطفا ابتدا صورتحساب مورد نظر را انتخاب نمایید !");
            else
                swal({
                    title: 'آیا مطمئنید ؟',
                    text: "صورتحساب مورد نظر حذف خواهد شد!",
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
                            text: 'صورتحساب مورد نظر با موفقیت حذف شد',
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
    
}



filter.initial();
entity.list.initial();













