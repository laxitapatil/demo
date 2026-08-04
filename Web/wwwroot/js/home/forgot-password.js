const ForgotPasswordModule = (() => {

    const AUTH_API = "authenticate";
    const $forgotPasswordForm = $("#forgotPasswordForm");
    const $userEmail = $("#userEmail");

    let validator;

    function forgotPasswordInit() {
        formValidation();
        forgotPasswordSubmit();
        $userEmail.focus();
    }

    function formValidation() {
        validator = $forgotPasswordForm.validate({
            errorElement: "span",
            rules: {
                userEmail: {
                    required: true,
                    email: true,
                    regex: /^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$/
                }
            },
            messages: {
                userEmail: {
                    required: "Please enter Email",
                    email: "Please enter a valid Email",
                    regex: "Please enter a valid Email format"
                }
            },
            errorPlacement: function (error, element) {
                if (element.parent('.input-group').length ||
                    element.prop('type') === 'checkbox' ||
                    element.prop('type') === 'radio') {
                    error.insertAfter(element.parent());
                } else {
                    error.insertAfter(element);
                }
                error.addClass("text-danger");
            }
        });

        // Add custom regex validation method if not already added
        if (!$.validator.methods.regex) {
            $.validator.addMethod("regex", function (value, element, regexp) {
                return this.optional(element) || regexp.test(value);
            }, "Please check your input format.");
        }
    }

    function forgotPasswordSubmit() {
        $forgotPasswordForm.on("submit", function (event) {
            event.preventDefault();

            if (!validator.form()) {
                errorToastr("Please enter a valid email address");
                return;
            }

            const email = $userEmail.val();

            // Store email in localStorage for reset password page
            localStorage.setItem("forgotPasswordEmail", email);

            const data = {
                email: email
            };

            callBasePostAPI(`${AUTH_API}/forget-password`, data, forgotPasswordSuccess, forgotPasswordError);
        });
    }

    function forgotPasswordSuccess(response) {
        if (response) {
            successToastr(response.message || "Password reset OTP has been sent to your email");

            // Redirect to reset password page
            setTimeout(() => {
                window.location.href = "/Home/ResetPassword";
            }, 1500);
        }
    }

    function forgotPasswordError(error) {
        commonError(error);
        // Clear the stored email if there's an error
        localStorage.removeItem("forgotPasswordEmail");
    }

    return {
        forgotPasswordInit
    };

})();

$(document).ready(ForgotPasswordModule.forgotPasswordInit);