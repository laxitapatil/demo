//var registerValidator;

//$(document).ready(function () {
//    registerInit();
//})

//function registerInit() {
//    registerFormValidation();
//    registerHtmlInit();
//}

//function registerHtmlInit() {
//    // $(".select2").select2({
//    //     width: '100%',
//    // });

//    var current_fs, next_fs, previous_fs; //fieldsets
//    var opacity;
//    var current = 1;
//    var steps = $("fieldset").length;

//    setProgressBar(current);

//    $(".next").click(function () {

//        current_fs = $(this).parents('fieldset');
//        next_fs = $(this).parents('fieldset').next();

//        //Add Class Active
//        $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

//        //show the next fieldset
//        next_fs.show();
//        //hide the current fieldset with style
//        current_fs.animate({ opacity: 0 }, {
//            step: function (now) {
//                // for making fielset appear animation
//                opacity = 1 - now;

//                current_fs.css({
//                    'display': 'none',
//                    'position': 'relative'
//                });
//                next_fs.css({ 'opacity': opacity });
//            },
//            duration: 500
//        });
//        setProgressBar(++current);
//    });

//    $(".previous").click(function () {
//        current_fs = $(this).parents('fieldset');
//        previous_fs = $(this).parents('fieldset').prev();

//        //Remove class active
//        $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");

//        //show the previous fieldset
//        previous_fs.show();

//        //hide the current fieldset with style
//        current_fs.animate({ opacity: 0 }, {
//            step: function (now) {
//                // for making fielset appear animation
//                opacity = 1 - now;

//                current_fs.css({
//                    'display': 'none',
//                    'position': 'relative'
//                });
//                previous_fs.css({ 'opacity': opacity });
//            },
//            duration: 500
//        });
//        setProgressBar(--current);
//    });

//    function setProgressBar(curStep) {
//        var percent = parseFloat(100 / steps) * curStep;
//        percent = percent.toFixed();
//        $(".progress-bar")
//            .css("width", percent + "%")
//    }
//}
//$.validator.addMethod("strongPassword", function (value, element) {
//    return this.optional(element) || /^(?=.*[a-z])(?=.*\d).{4,}$/.test(value);
//});
//$.validator.addMethod("emalValidation", function (value, element) {
//    return this.optional(element) || /\S+@\S+\.\S+/.test(value);
//}); 
//function registerFormValidation() {
//    registerValidator = $("#registerForm").validate({
//        errorElement: "span",
//        rules: {
//            company_name: {
//                required: true,
//                maxlength: 64
//            },
//            company_address: {
//                required: true,
//                maxlength: 128
//            },
//            company_pincode: {
//                required: true,
//                maxlength: 6
//            },
//            company_area: {
//                required: true
//            },
//            name: {
//                required: true,
//                maxlength: 64
//            },
//            phone: {
//                required: true,
//                maxlength: 10
//            },
//            email: {
//                required: true,
//                maxlength: 128,
//                emalValidation: true
//            },
//            Password: {
//                required: true,
//                strongPassword: true
//            },
//            confirm_password: {
//                required: true,
//                compare: "#Password"
//            }
//        },
//        messages: {
//            company_name: {
//                required: "Please enter Company name.",
//                maxlength: "Company name length must be less than 64 character."
//            },
//            company_address: {
//                required: "Please enter Company address.",
//                maxlength: "Company address length must be less than 128 character."
//            },
//            company_pincode: {
//                required: "Please enter Pincode.",
//                maxlength: "Pincode length must be less than 6 character."
//            },
//            company_area: {
//                required: "Please select Area."
//            },
//            name: {
//                required: "Please enter Name.",
//                maxlength: "Name length must be less than 64 character."
//            },
//            phone: {
//                required: "Please enter Phone.",
//                maxlength: "Phone length must be 10 character."
//            },
//            email: {
//                required: "Please enter Email.",
//                maxlength: "Email length must be 128 character.",
//                emalValidation: "Check your email address format."
//            },
//            Password: {
//                required: "Please enter Password.",
//                strongPassword: "Password must contain at least one letter and one number."
//            },
//            confirm_password: {
//                required: "Please enter Confirm Password.",
//                compare: "Confirm password does not match with Password."
//            }
//        },
//        errorPlacement: function (error, element) {
//            error.addClass("text-danger");

//            if (element.closest('.input-group').length > 0) {
//                element.closest('.input-group').after(error);
//            } else {
//                element.after(error);
//            }
//        }
//    })
//}

//$("#registerForm").submit(function (event) {
//    event.preventDefault();

//    if (!registerValidator.form()) {
//        const firstInvalidInput = $('#registerForm').find(':input.error').first();

//        // Move to that step if not visible
//        const errorFieldset = firstInvalidInput.closest('fieldset');
//        if (!errorFieldset.is(":visible")) {
//            const visibleFieldset = $("fieldset:visible");

//            visibleFieldset.animate({ opacity: 0 }, {
//                step: function (now) {
//                    visibleFieldset.css({ 'display': 'none', 'position': 'relative' });
//                    errorFieldset.css({ 'opacity': 1 }).show();
//                },
//                duration: 500
//            });

//            // Update progress bar
//            const index = $("fieldset").index(errorFieldset);
//            $("#progressbar li").removeClass("active");
//            $("#progressbar li").slice(0, index + 1).addClass("active");
//        }

//        // Scroll to first error field
//        $('html, body').animate({
//            scrollTop: firstInvalidInput.offset().top - 100
//        }, 500);

//        return;
//    }

//    data = {
//        company_name: event.target.company_name.value,
//        company_address: event.target.company_address.value,
//        pincode_id: parseInt(event.target.company_area.value),
//        name: event.target.name.value,
//        phone_no: event.target.phone.value,
//        email: event.target.email.value,
//        Password: event.target.Password.value,
//        subscription_type: parseInt(new URLSearchParams(window.location.search).get("type")) ? parseInt(new URLSearchParams(window.location.search).get("type")) : 1,
//    };

//    callPostAPI("authenticate/register", data, true, "registerSuccess", "commonError");
//})

//function registerSuccess(data) {
//    location.href = "/home/login";
//}

//$("#company_pincode").blur(function () {
//    if ($("#company_pincode").val() != '' && $("#company_pincode").val().length == 6)
//        callGetAPI("pincode/?code=" + $("#company_pincode").val(), false, "cityStateFill", "commonError");
//})

//function cityStateFill(data = []) {
//    if (data.length < 1) {
//        errorToastr("Please enter valid Pincode.");
//        return;
//    }

//    $("#company_city").val(data[0].city_name);
//    $("#company_state").val(data[0].state_name);
//    $("#company_pincode_id").val(data[0].id);

//    var v = "<option value=''>Select Area</option>";

//    if (data.length > 0)
//        $.each(data, function (i, v1) {
//            v += "<option value=" + v1.id + ">" + v1.area + "</option>";
//        });

//    $("#company_area").html(v);
//}