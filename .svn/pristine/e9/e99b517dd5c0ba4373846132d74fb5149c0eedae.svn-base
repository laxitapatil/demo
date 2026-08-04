const UserProfileModule = (() => {

    const PROFILE_API = "user/profile";
    const PROFILE_IMAGE_API = "user/user-profile-image";
    const PASSWORD_API = "user/change-password";

    const $profileForm = $("#userProfileForm");
    const $passwordForm = $("#changePassword");

    const $imageInput = $("#profileImageInput");
    const $imagePreview = $("#profileImagePreview");

    const DEFAULT_IMAGE = "/assets/images/users/avatar.png";

    let profileValidator = null;
    let passwordValidator = null;

    function profileInit() {
        profileValidation();
        passwordValidation();
        profileGet();
        profileBindEvents();
    }

    function profileGet() {
        callBaseGetAPI(PROFILE_API, profileFill, commonError);
    }

    function profileFill(res) {

        let data = res;

        if (typeof res === "string") {
            try {
                data = JSON.parse(res);
            } catch {
                errorToastr("Invalid profile response");
                return;
            }
        }

        if (!Array.isArray(data) || !data.length) {
            errorToastr("Profile data not found");
            return;
        }

        const user = data[0];

        $("#headerUserName").text(user.name);
        $("#userId").val(user.id || "");
        $("#name").val(user.name || "");
        $("#userEmail").val(user.email || "");
        $("#userContact").val(user.phone || "");

        setProfileImage(user.profile_image);
    }

    function profileBindEvents() {

        $profileForm.on("submit", profileSave);
        $passwordForm.on("submit", passwordChange);
        $imageInput.on("change", profileImageChange);

        $(document).on("shown.bs.modal", "#profileModal", profileGet);
    }

    function profileValidation() {
        profileValidator = $profileForm.validate({
            errorElement: "span",
            rules: {
                name: {
                    required: true,
                    notBlank: true,
                    maxlength: 64
                }
            },
            messages: {
                name: {
                    required: "Please enter Name",
                    notBlank: "Name cannot be blank",
                    maxlength: "Max 64 characters allowed"
                }
            },
            errorPlacement: function (error, element) {
                error.addClass("text-danger");
                error.insertAfter(element);
            }
        });
    }

    function passwordValidation() {
        passwordValidator = $passwordForm.validate({
            errorElement: "span",
            rules: {
                currentPassword: {
                    required: true,
                    notBlank: true
                },
                newPassword: {
                    required: true,
                    notBlank: true,
                    minlength: 6
                },
                confirmPassword: {
                    required: true,
                    equalTo: "#newPassword"
                }
            },
            messages: {
                currentPassword: "Enter current password",
                newPassword: {
                    required: "Enter new password",
                    notBlank: "Password cannot be blank",
                    minlength: "Minimum 6 characters required"
                },
                confirmPassword: {
                    required: "Confirm password",
                    equalTo: "Passwords do not match"
                }
            },
            errorPlacement: function (error, element) {
                error.addClass("text-danger d-block mt-1");
                error.insertAfter(element.closest(".input-group"));
            }
        });
    }

    function profileSave(e) {
        e.preventDefault();

        if (!profileValidator.form()) return;

        const data = {
            name: $("#name").val()
        };

        callBasePutAPI(PROFILE_API, data, profileSaveSuccess, commonError);
    }

    function profileSaveSuccess() {
        successToastr(MessageProvider.format(MessageProvider.UPDATE_SUCCESS, "Profile"));
        profileGet();
    }

    function profileImageChange() {
        const file = this.files[0];
        if (!file) return;

        const reader = new FileReader();
        reader.onload = e => $imagePreview.attr("src", e.target.result);
        reader.readAsDataURL(file);

        const formData = new FormData();
        formData.append("Profile_image", file);

        callBasePutFileAPI(
            PROFILE_IMAGE_API,
            formData,
            () => {
                successToastr(MessageProvider.format(MessageProvider.UPDATE_SUCCESS, "Profile Image"));
                profileGet();
            },
            commonError
        );
    }

    function passwordChange(e) {
        e.preventDefault();

        if (!passwordValidator.form()) return;

        const data = {
            userId: $("#userId").val(),
            currentPassword: $("#currentPassword").val(),
            newPassword: $("#newPassword").val()
        };

        callBasePutAPI(PASSWORD_API, data, passwordChangeSuccess, commonError);
    }

    function passwordChangeSuccess() {
        successToastr("Password changed successfully");
        $passwordForm[0].reset();
    }

    function setProfileImage(imageName) {

        const img = imageName
            ? generateImageURL("upload", "user", imageName) + "?v=" + Date.now()
            : DEFAULT_IMAGE;

        $(".header-profile-user").attr("src", img);
        $("#profileImagePreview").attr("src", img);
    }

    return {
        profileInit
    };

})();

$(document).ready(UserProfileModule.profileInit);