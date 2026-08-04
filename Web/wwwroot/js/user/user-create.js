const UserCreateModule = (() => {

    const USER_API = "user";
    const ROLE_API = "user/get-role";
    const PASSWORD_API = "user/put-password";

    const errorCallback = "commonError";

    let userValidator = null;
    let userId = null;
    let isEditMode = false;

    function userCreateInit() {
        userFormValidation();
        $("#role").on("change", function () {
            if (userValidator) {
                $(this).valid();
            }
        });
        $("#isActive").on("click", handleActiveToggle);
        $("#userForm").on("submit", userSave);
        $("#resetPasswordForm").on("submit", handleResetPassword);
        roleGet();
        userModalDisplay();
    }

    function roleGet() {

        callBaseGetAPI(ROLE_API,
            function (data) {

                if (typeof data === "string") {
                    try { data = JSON.parse(data); }
                    catch { return errorToastr("Invalid role response."); }
                }

                roleFill(data);
            },
            errorCallback
        );
    }

    function userModalDisplay() {

        $("#userModal").on("show.bs.modal", function () {

            if (isEditMode) {
                $("#myModalTitle").text("Edit User");
                $("#passwordGroup").hide();
                $("#contactNo").prop("disabled", true);
                $("#email").prop("disabled", true);
            } else {
                $("#myModalTitle").text("Add New User");
                $("#passwordGroup").show();
                $("#contactNo").prop("disabled", false);
                $("#email").prop("disabled", false);
            }
        });

        $("#userModal").on("hidden.bs.modal", function () {
            userId = null;
            isEditMode = false;
            resetForm();
        });
    }

    function openUserModal(id = null) {

        userId = id;
        isEditMode = !!id;

        if (isEditMode) {
            $("#contactNo").prop("disabled", true);
            $("#email").prop("disabled", true);
        } else {
            resetForm();
        }
        $("#userModal").modal("show");
    }


    function roleFill(data) {

        if (!Array.isArray(data) || data.length === 0) {
            errorToastr("No roles available.");
            return;
        }

        const $role = $("#role");

        $role.empty();
        $role.append("<option value='' disabled>Select User Role</option>");

        $.each(data, function (_, item) {
            $role.append(`<option value="${item.name}">${item.name}</option>`);
        });
    }

    function userFormValidation() {

        userValidator = $("#userForm").validate({
            errorElement: "span",
            rules: {
                fullName: {
                    required: true,
                    notBlank: true,
                    maxlength: 64
                },
                email: {
                    regex: /^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$/,
                    required: true,
                    maxlength: 64,
                    notBlank: true
                },
                contactNo: {
                    regex: /^[0-9]{10}$/,
                    required: true,
                    notBlank: true
                },
                password: {
                    required: function () {
                        return !isEditMode;
                    },
                    notBlank: true,
                    minlength: 6
                },
                role: {
                    required: true,
                    notBlank: true
                }
            },
            messages: {
                fullName: {
                    required: "Please enter Full Name.",
                    notBlank: "Full Name cannot be Blank",
                    maxlength: "Max length reached for Full Name."
                },
                email: {
                    regex: "Please enter valid Email.",
                    required: "Please enter Email.",
                    maxlength: "Max length reached for Email.",
                    notBlank: "Email cannot be Blank"
                },
                contactNo: {
                    regex: "Please enter valid Username /Contact No.",
                    required: "Please enter Username / Contact No.",
                    notBlank: "Username /Contact No cannot be Blank"
                },
                password: {
                    required: "Please enter Password.",
                    notBlank: "Password cannot be Blank",
                    minlength: "Enter atleast 6 characters."
                },
                role: {
                    required: "Please select User Role.",
                    notBlank: "Role cannot be Blank"
                }
            },
            errorPlacement: function (error, element) {

                if (element.parent('.input-group').length || element.prop('type') === 'checkbox') {
                    error.insertAfter(element.parent());
                }
                else if (element.hasClass("select2")) {
                    error.appendTo(element.parent());
                }
                else {
                    error.insertAfter(element);
                }

                error.addClass("text-danger");
            }
        });
    }
    function userSave(event) {

        event.preventDefault();

        if (!userValidator.form())
            return;

        const form = event.target;

        const data = {
            id: userId || null,
            name: form.fullName.value,
            email: form.email.value,
            phoneNumber: form.contactNo.value,
            role: $("#role").val(),
            is_active: form.isActive.checked
        };

        if (!isEditMode) {
            data.password = form.password.value;
        }

        if (isEditMode) {
            callBasePutAPI(USER_API, data, userUpdateSuccess, errorCallback, event);
        } else {
            callBasePostAPI(USER_API, data, userInsertSuccess, errorCallback, event);
        }
    }

    function userInsertSuccess() {

        successToastr(MessageProvider.format(MessageProvider.INSERT_SUCCESS, "User"));

        $("#userModal").modal("hide");

        UserModule.userInit();
    }

    function userUpdateSuccess() {

        successToastr(MessageProvider.format(MessageProvider.UPDATE_SUCCESS, "User"));

        $("#userModal").modal("hide");

        UserModule.userInit();
    }

    function userDataGet(data) {

        const user = Array.isArray(data) ? data[0] : data;

        if (!user) {
            errorToastr("User not found.");
            return;
        }

        $("#fullName").val(user.name);
        $("#email").val(user.email);
        $("#contactNo").val(user.phone || user.phoneNumber);
        $("#role").val(user.role).trigger("change");
        $("#isActive").prop("checked", user.is_active);
        $("#isActiveText").text(user.is_active ? "Active" : "Inactive");

        userId = user.id;
        isEditMode = true;
    }

    function userChangePasswordModal(id, username) {

        userId = id;

        $("#modalUsername").text(username);

        $("#changeUserPassModal").modal("show");
    }

    function handleResetPassword(event) {

        event.preventDefault();

        const password = $("#resetPassword").val();

        if (password.length < 6) {
            errorToastr("Password must be at least 6 characters.");
            return false;
        }

        const data = {
            userId: userId,
            newPassword: password
        };

        callBasePutAPI(PASSWORD_API, data, updatePasswordSuccess, errorCallback, event);
    }

    function updatePasswordSuccess() {

        successToastr(MessageProvider.format(MessageProvider.UPDATE_SUCCESS, "Password"));

        $("#resetPassword").val("");
    }

    function resetForm() {

        $("#userForm")[0].reset();

        $("#isActiveText").text("Inactive");

        $("#isActive").prop("checked", false);

        $("#contactNo").prop("disabled", false);
        $("#email").prop("disabled", false);

        if (userValidator) {
            userValidator.resetForm();
            $(".error").remove();
        }
    }

    function handleActiveToggle() {

        $("#isActiveText").text(this.checked ? "Active" : "Inactive");
    }

    return {
        userCreateInit,
        openUserModal,
        userChangePasswordModal,
        userDataGet
    };

})();

$(document).ready(UserCreateModule.userCreateInit);