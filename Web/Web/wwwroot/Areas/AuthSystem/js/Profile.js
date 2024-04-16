/*breadcrumb*/
var breadcrumb = [];
breadcrumb.push({ title: "پنل کاربری", link: "/Admin/Dashboard" });
breadcrumb.push({ title: "ویرایش پروفایل", link: "#" });


/*BaseAddress*/
var _area = 'Admin';
var _controller = 'Profile';
var _baseUrl = `/${_area}/${_controller}/`;


// عملیات مربوط به مدیریت پرسنل
var entity = {


    // آماده سازی فرم ها
    form: {
        initial: function () {
            $('#base-modal').removeClass('small-modal');
            $('.selectpicker').selectpicker('refresh');
            $.material.init();
            var provinceId = $('#ProvinceId option:selected').val();
            getCities(provinceId);
        },

        
        // اعتبار سنجی فرم ها
        validate: () => {
            var data = entity.form.collect();

            if (!data.FirstName) {
                showNotification('نام را وارد کنید!', 'danger')
                return null;
            }
            if (!data.LastName) {
                showNotification('نام خانوادگی را وارد کنید!', 'danger')
                return null;
            }
            if (!data.Mobile) {
                showNotification('تلفن همراه را وارد کنید!', 'danger')
                return null;
            }
            else {
                var regMobile = /^0\d{10}/;
                if (!regMobile.test(data.Mobile)) {
                    showNotification('فرمت تلفن همراه صحیح نیست', 'danger')
                    return null;                    
                }
            }
            if (!data.Username) {
                showNotification('نام کاربری را وارد کنید!', 'danger')
                return null;
            }
            if (!data.CityId) {
                showNotification('شهر را انتخاب کنید!', 'danger')
                return null;
            }            
            return data;

        },


        // جمع آوری دیتا از فرم ها
        collect: () => {
            var data = {
                __RequestVerificationToken: $('[name=__RequestVerificationToken]').val(),
                FirstName: $('#FirstName').val(),
                LastName: $('#LastName').val(),
                Mobile: $('#Mobile').val(),
                Username: $('#Username').val(),
                CityId: $('#CityId').val(),
            }
            return data;
        },


        // گرفتن لیست شهر های استان
        getCities: (id) => {
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


    },


    // ویرایش پروفایل
    edit: {
        save: function () {

            var targetUrl = _baseUrl + "Edit";

            var data = entity.form.validate();
            if (!data)
                return false;

            $.put(targetUrl,
                data,
                function (res) {
                    if (res.isSuccess)
                        showNotification('پروفایل شما با موفقیت ویرایش شد', 'success');
                    else
                        showNotification(res.message, 'danger');
                });
            return false;
        }
    },

}


