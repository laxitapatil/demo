const ResetPasswordModule = (() => {

    const AUTH_API = "authenticate";
    const $resetPasswordForm = $("#resetPasswordForm");
    const $userOtp = $("#userOtp");
    const $newPassword = $("#newPassword");
    const $confirmPassword = $("#confirmPassword");

    let validator;

    function resetPasswordInit() {
        // Check if email exists in localStorage
        if (!resetPasswordCheckEmail()) {
            errorToastr("Please request password reset again.");
            setTimeout(() => {
                window.location.href = "/Home/ForgotPassword";
            }, 2000);
            return;
        }

        formValidation();
        resetPasswordSubmit();
        $userOtp.focus();
    }

    function resetPasswordCheckEmail() {
        const email = localStorage.getItem("forgotPasswordEmail");
        return email !== null && email !== undefined && email.length > 0;
    }

    function formValidation() {
        validator = $resetPasswordForm.validate({
            errorElement: "span",
            rules: {
                userOtp: {
                    required: true,
                    digits: true,
                    minlength: 4,
                    maxlength: 6
                },
                newPassword: {
                    required: true,
                    minlength: 6
                },
                confirmPassword: {
                    required: true,
                    minlength: 6,
                    equalTo: "#newPassword"
                }
            },
            messages: {
                userOtp: {
                    required: "Please enter OTP",
                    digits: "OTP must contain only numbers",
                    minlength: "OTP must be at least 4 digits",
                    maxlength: "OTP must not exceed 6 digits"
                },
                newPassword: {
                    required: "Please enter New Password",
                    minlength: "Password must be at least 6 characters"
                },
                confirmPassword: {
                    required: "Please enter Confirm Password",
                    minlength: "Password must be at least 6 characters",
                    equalTo: "Confirm Password does not match with New Password"
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
    }

    function resetPasswordSubmit() {
        $resetPasswordForm.on("submit", function (event) {
            event.preventDefault();

            // Validate email exists
            if (!resetPasswordCheckEmail()) {
                errorToastr("Please request password reset again.");
                return;
            }

            // Validate form
            if (!validator.form()) {
                return;
            }

            const data = {
                email: localStorage.getItem("forgotPasswordEmail"),
                newPassword: $confirmPassword.val(),
                otp: $userOtp.val()
            };

            callBasePostAPI(`${AUTH_API}/reset-password`, data, resetPasswordSuccess, commonError);
        });
    }

    function resetPasswordSuccess(response) {
        if (response) {
            const message = response.message || response || "Password reset successfully";

            // Store success message for login page
            localStorage.setItem("successMessage", message);

            // Clear forgot password email
            localStorage.removeItem("forgotPasswordEmail");

            // Show success message
            successToastr(message);

            // Redirect to login page
            setTimeout(() => {
                window.location.href = "/";
            }, 1500);
        }
    }

    function resetPasswordResendOtp() {
        const email = localStorage.getItem("forgotPasswordEmail");

        if (!email) {
            errorToastr("Email not found. Please restart the password reset process.");
            return;
        }

        const data = { email: email };

        callBasePostAPI(`${AUTH_API}/forget-password`, data, resetPasswordResendSuccess, commonError);
    }

    function resetPasswordResendSuccess(response) {
        successToastr(response.message || "OTP has been resent to your email");
    }

    return {
        resetPasswordInit,
        resetPasswordResendOtp
    };

})();

$(document).ready(ResetPasswordModule.resetPasswordInit);