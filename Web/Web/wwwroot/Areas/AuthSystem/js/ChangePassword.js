/*breadcrumb*/
var breadcrumb = [];
breadcrumb.push({ title: "پنل کاربری", link: "/Admin/Dashboard" });
breadcrumb.push({ title: "تغییر کلمه عبور", link: "#" });


/*BaseAddress*/
var _area = 'Admin';
var _controller = 'Profile';
var _baseUrl = `/${_area}/${_controller}/`;


// عملیات مربوط به مدیریت پرسنل
var entity = {


    // آماده سازی فرم ها
    form: {
        // اعتبار سنجی فرم ها
        validate: () => {
            var data = entity.form.collect();

            if (!data.CurrentPassword || !data.Password || !data.RePassword || !data.Captcha) {
                showNotification('فیلدهای اطلاعاتی را کامل نمایید!', 'danger');
                return null;
            }

            var regPass = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,50}$/;
            if (!regPass.test(data.Password)) {
                showNotification('کلمه عبور را مطابق الگو وارد کنید!', 'danger');
                return null;
            }

            if (data.Password != data.RePassword) {
                showNotification('کلمه عبورهای وارد شده با هم مطابقت ندارند!', 'danger');
                return null;
            }

            return data;
        },


        // جمع آوری دیتا از فرم ها
        collect: () => {
            var data = {
                __RequestVerificationToken: $('[name=__RequestVerificationToken]').val(),
                CurrentPassword: $('#CurrentPassword').val(),
                Password: $('#Password').val(),
                RePassword: $('#RePassword').val(),
                Captcha: $('#captcha').val()
            }
            return data;
        },

        // قوی و ضعیف بودن پسورد بصورت گرافیکی
        isPasswordStrength: function (password) {
            var password_strength = $("#password-text");

            //TextBox left blank.
            if (password.length == 0) {
                password_strength.removeClass('btn-warning btn-success btn-danger').css('width', '1%').html('');
                return false;
            }

            //Regular Expressions.
            var regex = new Array();
            regex.push("[A-Z]"); //Uppercase Alphabet.
            regex.push("[a-z]"); //Lowercase Alphabet.
            regex.push("[0-9]"); //Digit.
            regex.push("[$@$!%*#?&]"); //Special Character.

            var passed = 0;

            //Validate for each Regular Expression.
            for (var i = 0; i < regex.length; i++) {
                if (new RegExp(regex[i]).test(password)) {
                    passed++;
                }
            }

            switch (passed) {
                case 0:
                    password_strength.removeClass('btn-warning btn-success').addClass('btn-danger').css('width', '30%').html('ضعیف');
                    break;
                case 1:
                    password_strength.removeClass('btn-warning btn-success').addClass('btn-danger').css('width', '30%').html('ضعیف');
                    break;
                case 2:
                    password_strength.removeClass('btn-warning btn-success').addClass('btn-danger').css('width', '30%').html('ضعیف');
                    break;
                case 3:
                    password_strength.removeClass('btn-danger btn-success').addClass('btn-warning').css('width', '60%').html('متوسط');
                    break;
                case 4:
                    password_strength.removeClass('btn-danger btn-warning').addClass('btn-success').css('width', '100%').html('قوی');
                    break;

            }

        },


        // تغییر تصویر کپچا
        changeCaptcha: () => {
            var d = new Date();
            $("#imgcpatcha").attr("src", "/Captcha/CaptchaImage?" + d.getTime());
            $('#captcha').val('');
        }
    },

    

    save: function (e) {
        e.preventDefault();
        var targetUrl = _baseUrl + "ChangePassword";

        var data = entity.form.validate();
        if (!data)
            return false;

        $.post(targetUrl,
            data,
            function (res) {
                if (res.isSuccess) {
                    showNotification('کلمه عبور با موفقیت ویرایش شد.', 'success');
                    $('#CurrentPassword').val('');
                    $('#Password').val('');
                    $('#RePassword').val('');
                }
                else 
                    showNotification(res.errors.join(' | '), 'danger');
            }).done(() => entity.form.changeCaptcha());
        return false;
    }


}

