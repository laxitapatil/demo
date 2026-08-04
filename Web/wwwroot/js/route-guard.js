const RouteGuard = (() => {

    const routes = [
        { path: "/state", roles: ["Admin"] },
        { path: "/bank", roles: ["Admin"] },
        { path: "/user", roles: ["Admin"] },
        { path: "/casetype", roles: ["Admin"] },
        { path: "/propertytype", roles: ["Admin"] },

        { path: "/case", roles: ["Admin", "Visitor", "Report Maker"] },
        { path: "/dashboard", roles: ["Admin", "Visitor", "Report Maker"] },
        { path: "/case-analysis", roles: ["Admin", "Report Maker"] }
    ];

    function checkAccess() {

        const currentPath = window.location.pathname.toLowerCase();
        const roles = AuthModule.getRolesFromToken();

        if (!roles.length) {
            AuthModule.logout();
            return;
        } // not logged in case

        const route = routes.find(r => currentPath === r.path || currentPath.startsWith(r.path + "/"));

        if (!route) return; // no restriction

        const hasAccess = roles.some(role => route.roles.includes(role));

        if (!hasAccess) {

            sessionStorage.setItem("unauthorized_msg", "You are not authorized to access this page");
            window.location.href = "/dashboard";
        }
    }

    return {
        checkAccess
    };

})();