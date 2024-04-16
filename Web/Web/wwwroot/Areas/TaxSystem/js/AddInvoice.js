/*breadcrumb*/
var breadcrumb = [];
breadcrumb.push({ title: "پنل کاربری", link: "/AuthSystem/Dashboard" });
breadcrumb.push({ title: "صورتحساب‌ ها", link: "#" });


/*BaseAddress*/
var _area = 'TaxSystem';
var _controller = 'AddInvoice';
var _baseUrl = `/${_area}/${_controller}/`;


// عملیات مربوط به مدیریت صورتحساب ها
var entity = {
    // فرم افزودن و ویرایش
    form: {
        //آماده سازی فرم ها
        initial: () => {
            $.material.init();
            entity.form.invoiceDate.initial();
        },

        invoiceDate: {

            // آبجکت vue
            obj: null,

            //راه اندازی تقویم تاریخ شروع
            initial: function () {
                if ($('#VueInvoiceDate').length == 0)
                    return;
                var sdate = '';// moment().subtract(1, 'd').format('jYYYY/jMM/jDD');
                this.obj = new Vue({
                    el: '#VueInvoiceDate',
                    data: {
                        date: sdate
                    },
                    components: {
                        DatePicker: VuePersianDatetimePicker
                    }
                });
            }
            
        },


        collect: function () {
            var data = {
                __RequestVerificationToken: $("[name=__RequestVerificationToken]").val(),
                invoiceNumber: $("#InvoiceNumber").val(),
                invoiceDate: $("[name=InvoiceDate]").val(),
                buyerName: $("#BuyerName").val(),
                buyerNationalCode: $("#BuyerNationalCode").val(),
                buyerEconomicCode: $("#BuyerEconomicCode").val(),
                buyerPhone: $("#BuyerPhone").val(),
                buyerMobile: $("#BuyerMobile").val(),
                buyerAddress: $("#BuyerAddress").val(),
                buyerPostalCode: $("#BuyerPostalCode").val(),
                BuyerIsRealOrLegal: $("#BuyerIsRealOrLegal").val(),
                TaxInvoiceType: $("#TaxInvoiceType").val(),
                PayType: $("#PayType").val(),
                TaxInvoicePattern: $("#TaxInvoicePattern").val(),
                TaxInvoiceSubject: $("#TaxInvoiceSubject").val(),
                description: $("#Description").val(),
                cashAmount: $("#CashAmount").val(),
                InvoiceItems: invoiceItem.collect(),

            };

            var patternId = $('#TaxInvoicePattern').val();
            if (patternId == TaxInvoicePatternEnum.Pattern4)
                data.ContractId = $("#ContractId").val();
            return data;
        },


        // اعتبار سنجی فرم ها
        validate: () => {
            var data = entity.form.collect();
            console.log('validate data', data)
            if (!data.invoiceNumber) {
                showNotification("شماره فاکتور اجباری است!", 'danger');
                return null;
            }

            if (!data.invoiceDate) {
                showNotification("تاریخ صدور فاکتور اجباری است!", 'danger');
                return null;
            }

            if (!data.cashAmount) {
                showNotification("مبلغ پرداختی نقدی اجباری است!", 'danger');
                return null;
            }

            if (!data.buyerName) {
                showNotification("نام خریدار اجباری است!", 'danger');
                return null;
            }

            if (!data.buyerNationalCode) {
                showNotification("کد/شناسه ملی خریدار اجباری است!", 'danger');
                return null;
            }

            //if (!data.buyerEconomicCode) {
            //    showNotification("شماره اقتصادی خریدار اجباری است!", 'danger');
            //    return null;
            //}

            //if (!data.buyerMobile) {
            //    showNotification("موبایل خریدار اجباری است!", 'danger');
            //    return null;
            //}

            if (!data.InvoiceItems) {
                showNotification("آیتم های صورت حساب اجباری است!", 'danger');
                return null;
            }

            var patternId = $('#TaxInvoicePattern').val();
            if (patternId == TaxInvoicePatternEnum.Pattern4 && !data.ContractId)
            {
                showNotification("برای صورتحساب با الگوی پیمانکاری، شماره قرارداد الزامی است!", 'danger');
                return null;
            }

            return data;

        },


        changePattern: () => {
            var patternId = $('#TaxInvoicePattern').val();
            if (patternId == TaxInvoicePatternEnum.Pattern4)
                $('.contract-id-section').slideDown();
            else
                $('.contract-id-section').slideUp();
        }
    },


    create: {
        save: () => {
            var data = entity.form.validate();
            console.log(data);
            if (!data)
                return false;
            $.post(_baseUrl + 'Index', data, (res) => {
                if (res.isSuccess) {
                    swal({
                        title: 'انجام شد',
                        text: "صورتحساب شما با موفقیت ثبت گردید.",
                        type: 'success',
                        showCancelButton: true,
                        confirmButtonClass: 'btn btn-success',
                        cancelButtonClass: 'btn btn-primary',
                        confirmButtonText: 'ثبت صورتحساب جدید',
                        cancelButtonText: 'مشاهده لیست صورتحساب',
                        buttonsStyling: false
                    }).then(function (isConfirm) {
                        if (isConfirm)
                            window.location.href = window.location.href;
                    }).catch(() => {
                        window.location.href = "/TaxSystem/Invoices";
                        swal.noop;
                    });
                }
                else
                    showNotification(res.message, "danger");
                return false;
            });
            return false;
        }
    }

}

var invoiceItem = {
    add: function () {
        var data = {
            name: $("#invoiceItemName").val(),
            code: $("#invoiceItemCode").val(),
            quantity: $("#invoiceItemQuantity").val(),
            countingUnitName: $("#invoiceItemCountingUnitName").val(),
            unitPrice: $("#invoiceItemUnitPrice").val(),
            discountAmount: $("#invoiceItemDiscountAmount").val(),
            taxRate: $("#invoiceItemTaxRate").val(),
            taxAmount: $("#invoiceItemTaxAmount").val(),
            otherTaxAmount: $("#invoiceItemOtherTaxAmount").val()
        }

        if (!data.name) {
            showNotification("نام کالا/خدمت اجباری است!", 'danger');
            return false;
        }

        if (!data.code) {
            showNotification("کد کالا/خدمت اجباری است!", 'danger');
            return false;
        }

        if (!data.quantity) {
            showNotification("تعداد کالا/خدمت اجباری است!", 'danger');
            return false;
        }

        //if (!data.countingUnitName) {
        //    showNotification("نام واحد کالا/خدمت اجباری است!", 'danger');
        //    return false;
        //}

        if (!data.unitPrice) {
            showNotification("قیمت واحد کالا/خدمت اجباری است!", 'danger');
            return false;
        }

        if (!data.discountAmount) {
            showNotification("تخفیف واحد کالا/خدمت اجباری است!", 'danger');
            return false;
        }

        if (!data.taxRate) {
            showNotification("تخفیف واحد کالا/خدمت اجباری است!", 'danger');
            return false;
        }


        invoiceItem.insertTableRow(data);

        $("#invoiceItemName").val('');
        $("#invoiceItemCode").val('');
        $("#invoiceItemQuantity").val('');
        $("#invoiceItemCountingUnitName").val('');
        $("#invoiceItemUnitPrice").val('');
        $("#invoiceItemDiscountAmount").val('');
        $("#invoiceItemTaxRate").val('');
        $("#invoiceItemTaxAmount").val('');
        $("#invoiceItemOtherTaxAmount").val('');
    },


    insertTableRow: (date) => {
        var tr = `<tr> 
        <td>${date.name}</td>
        <td>${date.code}</td>
        <td>${date.quantity}</td>
        <td>${date.countingUnitName}</td>
        <td>${date.unitPrice}</td>
        <td>${date.discountAmount}</td>
        <td>${date.taxRate}</td>
        <td>${date.taxAmount}</td>
        <td>${date.otherTaxAmount}</td>
        <td><a class="btn btn-simple btn-danger btn-icon" onclick="invoiceItem.delete(this)"><i class="material-icons">close</i></a></td>
        </tr>`;

        $('#invoice-item-table tbody').append(tr);
    },


    delete: (el) => {
        $(el).parents('tr').remove();
    },

    collect: () => {
        var trs = $('#invoice-item-table');
        var items = [];
        for (var i = 0; i < trs.length; i++) {
            var data = {
                name: $(trs[i]).find('td:nth-child(1)').text(),
                code: $(trs[i]).find('td:nth-child(2)').text(),
                quantity: $(trs[i]).find('td:nth-child(3)').text(),
                countingUnitName: $(trs[i]).find('td:nth-child(4)').text(),
                unitPrice: $(trs[i]).find('td:nth-child(5)').text(),
                discountAmount: $(trs[i]).find('td:nth-child(6)').text(),
                taxRate: $(trs[i]).find('td:nth-child(7)').text(),
                taxAmount: $(trs[i]).find('td:nth-child(8)').text(),
                otherTaxAmount: $(trs[i]).find('td:nth-child(9)').text(),
            }

            items.push(data);
        }
        return items;

    }

}

entity.form.initial()