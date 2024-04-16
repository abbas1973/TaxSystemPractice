
// وضعیت حقیقی / حقوقی خریدار
const BuyerIsRealOrLegalEnum = Object.freeze({ "Real": 1, "Legal": 2 });
const InvoicePayTypeEnum = Object.freeze({ "Cash": 1, "Credit": 2, "CashCredit": 3 });
const TaxInvoiceTypeEnum = Object.freeze({ "Type1": 1, "Type2": 2, "Type3": 3 });
const TaxInvoicePatternEnum = Object.freeze({ "Pattern1": 1, "Pattern2": 2, "Pattern3": 3, "Pattern4": 4, "Pattern5": 5, "Pattern6": 6, "Pattern7": 7 });
const TaxInvoiceSubjectTypeEnum = Object.freeze({ "Main": 1, "Edited": 2, "Cancelled": 3, "Rejected": 4});


var showAjaxLoadingModal = true;
$(document).ajaxStart(function (e) {
    if (showAjaxLoadingModal == true) {
        swal({
            title: 'لطفا صبر کنید',
            animation: false,
            customClass: "animated fadeInUp loading-swal",
            allowOutsideClick: false,
            onOpen: function () {
                swal.showLoading();
            }
        });
    }
}).ajaxStop(function () {
    showAjaxLoadingModal = true;
    if ($('.loading-swal').length > 0) {
        swal.close("loading-swal");
    }
    if ($('form').length > 0)
        $('form').attr('autocomplete', 'off');
});


// اجکس هایی که نیازی به نشان دادن مدال "لطفا صبر کنید" ندارند
var allowedList = []
$(document).ajaxSend(function (event, jqxhr, settings) {
    var url = settings.url.toLowerCase();
    if (typeof startSubmition == 'undefined' || startSubmition == false) {
        for (var i = 0; i < allowedList.length; i++)
            if (url.indexOf(allowedList[i]) >= 0) {
                swal.close("loading-swal");
            }
    }
});


// خطای سراسری اجکس ها
$(document).ajaxError(function myErrorHandler(event, xhr, ajaxOptions, thrownError) {
    if (xhr.responseJSON && xhr.responseJSON.Errors)
        showNotification(xhr.responseJSON.Errors.join(' | '), 'danger');
    else
        showNotification("عملیات با خطا همراه بوده است. مجددا اقدام کنید.", 'danger');
});


// فوکس روی اینپوت دراپدون هنگام باز شدن
$('body').on("click", '.bootstrap-select button.dropdown-toggle', function () {
    if (!$(this).parent().hasClass("open")) {
        var input = $(this).parent().find(".bs-searchbox input");
        if (input && input.length > 0)
            $(input).focusin();
    }
});




//باز و بسته کردن مدال
var modal = {
    //باز کردن مدال
    open: function (target) {
        if (!target)
            $("#base-modal").modal("show");
        else
            $(target).modal("show");
    },

    //بستن مدال
    close: function (target) {
        if (!target)
            $("#base-modal").modal("hide");
        else
            $(target).modal("hide");

        if ($('.modal.in').length > 0)
            $('.modal.in').css('overflow-y', 'scroll');
    },
}




// تعیین مکان اسکرول به یک المان خاص و یا اول صفحه
var setScrollPosition = function (el) {
    if (!el || $(el).length == 0)
        el = '.main-content';
    $(".main-panel").mCustomScrollbar("scrollTo", el);
}





var menus = {
    // منو انتخاب شده
    active: null,

    // لود کردن منو ها از سرور یا لوکال استوریج
    load: function () {
        var _menus = localStorage.getItem('menu');
        if (_menus) {
            menus.init(_menus);
            return;
        }

        $.get('/shared/ClientMenus/LoadMenu',
            { CController: controller, CAction: action, UserId: UserId },
            function (res) {
                localStorage.setItem('menu', res);
                menus.init(res);
            }).fail(function () { alert('بارگذاری منوها با خطا همراه بوده است.') })
    },


    // راه اندازی منوها و نمایش منو انتخاب شده
    init: function (res) {
        $('.sidebar-wrapper .main-nav').html(res);
        
        //باز کردن والدهای منو انتخاب شده
        menus.active = $(`.nav-item.${controller}-${action}`);
        menus.makeActive();
    },


    // انتخاب منو فعال و باز کردن سر دسته ها
    makeActive: () => {
        $(menus.active).addClass('active');
        $(`.nav-item.active`).parents('.nav-item').addClass('active');
        $('.nav-item.active').parents('.collapse').addClass('in');
        // قرار دادن اسکرول منو کناری روی ایتم انتخاب شده
        menus.setScroll();
    },


    // جستجو منو ها
    search: function (el) {
        var srch = $(el).val();
        $('.sidebar-wrapper .main-nav').removeClass('search');
        $('.sidebar-wrapper .main-nav .nav-item').removeClass('search').removeClass('active');
        $('.sidebar-wrapper .main-nav .collapse').removeClass('in');

        if (!srch) {
            menus.makeActive();
            return;
        }

        $('.sidebar-wrapper .main-nav').addClass('search');
        var navs = $(`.sidebar-wrapper .main-nav .nav-item`);
        for (var i = 0; i < navs.length; i++) {
            if ($(navs[i]).text().indexOf(srch) > 0) {
                $(navs[i]).addClass('search');
                $(navs[i]).parents('.nav-item').addClass('active');
                $(navs[i]).parents('.collapse').addClass('in');
            }
        }
    },


    setScroll: function () {
        // قرار دادن اسکرول منو کناری روی ایتم انتخاب شده
        $('.sidebar .sidebar-wrapper').mCustomScrollbar("scrollTo", '.sidebar .nav-item.active > .nav-link');
    }
}


if ($('.sidebar').length > 0)
    menus.load();






//فیلتر های مربوط به جستجو
var filter = {
    // باز و بسته کردن پنجره فیلتر
    toggle: function () {
        if ($(".filter-caption").hasClass('closed')) {
            $(".filter-container").slideDown();
            $(".filter-caption").removeClass('closed');
        }
        else {
            $(".filter-container").slideUp();
            $(".filter-caption").addClass('closed');
        }
    }
}





// توابع عمومی استفاده شده در فرم ها
var form = {

    // قرار دادن ویرگول بین قیمت
    renderPrice: function (el, event) {
        // skip for arrow keys
        if (event)
            if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(el).val(function (index, value) {
            var neg = '';
            if (value && parseInt(value) < 0) {
                neg = '-';
                value = value.substr(1, value.length - 1);
            }
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                + neg
                ;
        });
    },



    // گرفتن فرمت قیمت
    getPriceFormat: function (price) {
        if (!price)
            return price;
        price = price.toString();
        if (price) {
            var neg = '';
            if (parseInt(price) < 0) {
                neg = '-';
                price = price.substr(1, price.length - 1);
            }
            price = price
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",") + neg;
            return price;
        }
        else if (price == "0")
            return '0';
        else
            return '';
    }
}




var renderPrice = function (el, event) {
    // skip for arrow keys
    if (event)
        if (event.which >= 37 && event.which <= 40) return;

    // format number
    $(el).val(function (index, value) {
        var neg = '';
        if (value && parseInt(value) < 0) {
            neg = '-';
            value = value.substr(1, value.length - 1);
        }
        return value
            .replace(/\D/g, "")
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
            + neg
            ;
    });
};

var getPriceFormat = function (price) {
    if (!price)
        return price;
    price = price.toString();
    if (price) {
        var neg = '';
        if (parseInt(price) < 0) {
            neg = '-';
            price = price.substr(1, price.length - 1);
        }
        price = price
            .replace(/\D/g, "")
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",") + neg;
        return price;
    }
    else if (price == "0")
        return '0';
    else
        return '';
};





// تنظیمات چارت ها
var chartSetting = {

    // گرفتن خروجی از چارت
    export: {
        setting: function (showMoreInfo, mainContainer) {
            var option = {
                fallbackToExportServer: false,
                //allowHTML: true,
                buttons: {
                    contextButton: {
                        menuItems: [{
                            text: 'نمایش تمام صفحه',
                            onclick: function () {
                                this.fullscreen.toggle();
                            }
                        }, {
                            text: 'پرینت',
                            onclick: function () {
                                this.print();
                            }
                        }, {
                            separator: true
                        }, {
                            text: 'دانلود png',
                            onclick: function () {
                                this.exportChartLocal();
                            }
                        }, {
                            text: 'دانلود jpeg',
                            onclick: function () {
                                this.exportChartLocal({
                                    type: 'image/jpeg'
                                });
                            }
                        }, {
                            text: 'دانلود pdf',
                            onclick: function () {
                                this.exportChartLocal({
                                    type: 'application/pdf'
                                });
                            }
                        }, {
                            text: 'دانلود svg',
                            onclick: function () {
                                this.exportChartLocal({
                                    type: 'image/svg+xml'
                                });
                            }
                        }]
                    }
                }
            };

            if (showMoreInfo) {
                option.buttons.contextButton.menuItems.push({ separator: true });
                option.buttons.contextButton.menuItems.push({
                    text: 'نمایش اطلاعات تکمیلی',
                    onclick: function () {
                        $(mainContainer).find('.more-info').toggleClass('hide');
                        if ($(mainContainer).find('.more-info').hasClass('hide'))
                            $(mainContainer).find('.highcharts-menu-item:last-of-type').html('نمایش اطلاعات تکمیلی');
                        else
                            $(mainContainer).find('.highcharts-menu-item:last-of-type').html('عدم نمایش اطلاعات تکمیلی');
                    }
                });
            }
            return option;
        },


        //چاپ کردن چارت
        print: function (el) {
            var chart = $(el).highcharts();
            chart.print();
        },


        // نمایش تمام صفحه
        fullscreen: function (el) {
            var chart = $(el).highcharts();
            chart.fullscreen.toggle();
        },


        //ذخیره بصورت PNG
        exportPNG: function (el) {
            var chart = $(el).highcharts();
            chart.exportChartLocal({ allowHTML: true });
        },


        // ذخیره بصورت JPEG
        exportJPEG: function (el) {
            var chart = $(el).highcharts();
            chart.exportChartLocal({
                type: 'image/jpeg'
            });
        },


        ///ذخیره بصورت PDF
        exportPDF: function (el) {
            var chart = $(el).highcharts();
            chart.exportChartLocal({
                type: 'application/pdf'
            });
        },

        ///ذخیره بصورت SVG
        exportSVG: function (el) {
            var chart = $(el).highcharts();
            chart.exportChartLocal({
                type: 'image/svg+xml'
            });
        },


        // نمایش اطلاعات تکمیلی چارت
        moreInfo: function (el, mainContainer) {
            $(mainContainer).find('.more-info').toggleClass('hide');
            setTimeout(function () {
                if ($(mainContainer).find('.more-info').hasClass('hide'))
                    $(el).html('نمایش اطلاعات تکمیلی');
                else
                    $(el).html('عدم نمایش اطلاعات تکمیلی');
            }, 500);

        }
    },


    // عنوان چارت
    titleSetting: function (title) {
        var option = {
            text: title,
            style: {
                fontSize: '14px',
                direction: 'rtl',
                textAlign: 'center',
                //lineHeight: '40px;',
                fontWeight: 'bold',
                fontFamily: 'IranSans'
            }
        };
        return option;
    },


    // زیر عنوان چارت
    subtitleSetting: function (text) {
        var option = {
            text: text,
            style: {
                fontSize: '12px',
                direction: 'rtl;',
                textAlign: 'center',
                fontWeight: 'bold',
                fontFamily: 'IranSans'
            }
        };
        return option;
    },


    //چارت از نوع میله ای
    column: {
        xAxis: function (cats) {
            var model = {
                type: 'category',
                categories: cats,
                labels: {
                    rotation: 0,
                    style: {
                        fontSize: '12px',
                        textAlign: "center",
                        direction: 'rtl',
                        fontFamily: 'IranSans',
                        lineHeight: '20px',
                    },
                    //useHTML: true
                },
                reversed: true
            };
            return model;
        },


        yAxis: function (title) {
            var model = {
                min: 0,
                title: {
                    text: title,
                    //useHTML: true,
                    style: {
                        fontSize: '13px',
                        fontWeight: 'bold',
                        fontFamily: 'IranSans'
                    }
                },
                opposite: true
            };
            return model;
        },



        series: function (title, values) {
            var model = [{
                name: title,
                data: values,
                dataLabels: {
                    enabled: true,
                    //rotation: 0,
                    //color: '#FFFFFF',
                    //align: 'center',
                    format: '{point.y:.1f}', // one decimal
                    //y: 35, // 10 pixels down from the top
                    style: {
                        fontSize: '13px'
                    }
                }
            }];
            return model;
        }
    }

}







// نمایش base64 بصورت پی دی اف در پنجره جدید
var printPdfPreview = (data, type = 'application/pdf') => {
    let blob = null;
    blob = this.b64toBlob(data, type);
    const blobURL = URL.createObjectURL(blob);
    const theWindow = window.open(blobURL);
    const theDoc = theWindow.document;
    const theScript = document.createElement('script');
    function injectThis() {
        window.print();
    }
    theScript.innerHTML = `window.onload = ${injectThis.toString()};`;
    theDoc.body.appendChild(theScript);
};

var b64toBlob = (content, contentType) => {
    contentType = contentType || '';
    const sliceSize = 512;
    // method which converts base64 to binary
    const byteCharacters = window.atob(content);

    const byteArrays = [];
    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        const slice = byteCharacters.slice(offset, offset + sliceSize);
        const byteNumbers = new Array(slice.length);
        for (let i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }
    const blob = new Blob(byteArrays, {
        type: contentType
    }); // statement which creates the blob
    return blob;
};






// گرفتن عرض صفحه
if ($(window).width() < 768) {
    $('.filter .filter-caption').addClass('closed');
}





// ولیدیت کردن با رگولار اکسپرشن
var validateRole = {
    mobile: {
        regex: /^[\u06F0|0][\u06F0-\u06F90-9]{10}/g,
        minLength: 11,
        maxLength: 11,

        isMatch: function (text) {
            if (!text)
                return { status: false, message: 'الزامی است!' };
            if (text.length < this.minLength)
                return { status: false, message: 'حداقل ' + this.minLength + ' کاراکتر باشد!' };
            if (text.length > this.maxLength)
                return { status: false, message: 'حداکثر ' + this.maxLength + ' کاراکتر باشد!' };
            if (!text.match(this.regex))
                return { status: false, message: 'فرمت وارد شده صحیح نیست!' };
            return { status: true };
        }
    },


    email: {
        regex: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,

        isMatch: function (text) {
            if (!text)
                return { status: false, message: 'الزامی است!' };
            if (!text.match(this.regex))
                return { status: false, message: 'فرمت وارد شده صحیح نیست!' };
            return { status: true };
        }
    },


    number: {
        regex: /^[0-9]+$/,

        isMatch: function (text) {
            if (!text)
                return { status: false, message: 'الزامی است!' };
            if (!text.match(this.regex))
                return { status: false, message: 'فقط عدد مجاز است!' };
            return { status: true };
        }
    },



    minLength: {
        isMatch: function (text, minLength) {
            if (!text)
                return { status: false, message: 'الزامی است!' };
            if (text.length < minLength)
                return { status: false, message: 'حداقل ' + minLength + ' کاراکتر باشد!' };
            return { status: true };
        }
    },


    maxLength: {
        isMatch: function (text, maxLength) {
            if (!text)
                return { status: false, message: 'الزامی است!' };
            if (text.length > maxLength)
                return { status: false, message: 'حداکثر ' + maxLength + ' کاراکتر باشد!' };
            return { status: true };
        }
    },

}



// پشتیبانی  جی کوئری از put  و delete
jQuery.each(["put", "delete"], function (i, method) {
    jQuery[method] = function (url, data, callback, type) {
        if (jQuery.isFunction(data)) {
            type = type || callback;
            callback = data;
            data = undefined;
        }

        return jQuery.ajax({
            url: url,
            type: method,
            dataType: type,
            data: data,
            success: callback
        });
    };
});


