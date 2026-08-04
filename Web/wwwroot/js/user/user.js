const UserModule = (() => {

    const USER_API = "user";
    const USER_TABLE = "#tblUser";

    const $userTable = $(USER_TABLE);
    let dataTable = null;


    function userInit() {
        userGet();
    }


    function userGet() {
        callBaseGetAPI(USER_API,
            function (data) {

                if (typeof data === "string") {
                    try {
                        data = JSON.parse(data);
                    } catch (e) {
                        errorToastr("Invalid server response.");
                        return;
                    }
                }

                userFill(data);
            },
            commonError
        );
    }

    function userFill(data) {

        if (!Array.isArray(data)) {
            errorToastr("Data not found.");
            return;
        }

        data.forEach((item, i) => item.rowNo = i + 1);

        if (dataTable) {
            dataTable.clear().rows.add(data).draw();
            return;
        }

        dataTable = setDataTable($userTable, {
            scrollY: '70vh',
            scrollCollapse: false,
            scrollX: true,
            responsive: true,
            autoWidth: false,
            data: data,
            columns: [

                // ACTION COLUMN
                {
                    data: "id",
                    orderable: false,
                    render: function (id, type, row) {

                        return `
                            <div class="d-flex align-items-center gap-2 justify-content-center">

                                <a href="javascript:void(0);" 
                                   class="link-info"
                                   onclick="UserModule.userEdit('${id}')"
                                   title="Edit User">
                                    <i class="fi fi-rr-pencil"></i>
                                </a>

                                <a href="javascript:void(0);" 
                                   class="link-success"
                                   onclick="UserModule.userChangePassword('${id}', '${row.name ?? ""}')"
                                   title="Change Password">
                                    <i class="fi fi-rr-lock"></i>
                                </a>

                            </div>
                        `;
                    }
                },

                // ROW NUMBER
                {
                    data: null,
                    render: (_, __, ___, meta) =>
                        meta.row + meta.settings._iDisplayStart + 1
                },

                { data: "name" },
                { data: "role" },
                { data: "phone" },
                { data: "email" },

                // STATUS
                {
                    data: "is_active",
                    render: function (active) {

                        if (active === true || active === 1 || active === "1") {
                            return `<span class="text-success fw-semibold">Active</span>`;
                        }

                        if (active === false || active === 0 || active === "0") {
                            return `<span class="text-danger fw-semibold">Inactive</span>`;
                        }

                        return `<span class="text-muted">—</span>`;
                    }
                }
            ],
            initComplete: function () {
                applyColumnFilters(this.api(), {
                    noFilterColumns: [0, 1, 3, 6]
                });
            }
        });
    }

    function userEdit(id) {

        callBaseGetAPI(
            `${USER_API}?id=${encodeURIComponent(id)}`,
            function (data) {

                if (typeof data === "string") {
                    try {
                        data = JSON.parse(data);
                    } catch (e) {
                        errorToastr("Invalid server response.");
                        return;
                    }
                }

                if (!data) {
                    errorToastr("User not found.");
                    return;
                }

                UserCreateModule.userDataGet(data);
                UserCreateModule.openUserModal(id);
            },
            commonError
        );
    }

    function userDelete(id) {

        if (!id) return;

        callBaseDeleteAPI(
            `${USER_API}/${id}`,
            function (res) {
                successToastr(res);
                userGet();
            },
            commonError
        );
    }

    function userChangePassword(id, username) {
        UserCreateModule.userChangePasswordModal(id, username);
    }
    function userAutoLogin(userId) {

        const payload = {
            userName: userId,
            password: "Auto-Password"
        };

        callPostAPI("authenticate/auto-login",
            payload,
            function (res) {

                if (res?.message) {
                    errorToastr(res.message);
                    return;
                }

                AuthModule.setUser({
                    id: res.id,
                    name: res.name,
                    email: res.email,
                    phone: res.phone_number,
                    role: res.role,
                    companyId: res.company_id
                });

                AuthModule.setToken(res.token);

                location.reload();
                window.open("/dashboard", "_blank");
            },
            commonError
        );
    }

    return {
        userInit,
        userEdit,
        userChangePassword,
        userDelete,
        userAutoLogin
    };

})();

$(document).ready(function () {
    const roles = AuthModule.getRolesFromToken();

    if (!roles.includes("Admin")) {
        return;
    }

    UserModule.userInit();
});