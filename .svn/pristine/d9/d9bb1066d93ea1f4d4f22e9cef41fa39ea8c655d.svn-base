const AuthModule = (() => {

    const TOKEN_KEY = "token";
    const REFRESH_TOKEN_KEY = "refreshToken";
    const USER_KEY = "user";

    function getToken() {
        return localStorage.getItem(TOKEN_KEY) || "";
    }

    function setToken(token) {
        localStorage.setItem(TOKEN_KEY, token);
    }

    function getRefreshToken() {
        return localStorage.getItem(REFRESH_TOKEN_KEY) || "";
    }

    function setRefreshToken(refreshToken) {
        localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
    }

    function setUser(userObj) {
        localStorage.setItem(USER_KEY, JSON.stringify(userObj));
    }

    function getUser() {
        const data = localStorage.getItem(USER_KEY);
        try { return JSON.parse(data) || null; }
        catch { return null; }
    }

    function getRolesFromToken() {

        const token = getToken();
        if (!token) return [];

        try {
             // atob => decode base64 string, split => get the payload part of the token
            //and convert into json object to access the role claim
            const payload = JSON.parse(atob(token.split('.')[1]));

            const roles = payload["role"] || payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
            return Array.isArray(roles) ? roles : [roles];
        }
        catch {
            return [];
        }
    }
    function logout() {
        localStorage.clear();
        window.location.href = "/";
    }

    return {
        getToken,
        setToken,
        getRefreshToken,
        setRefreshToken,
        getUser,
        setUser,
        getRolesFromToken,
        logout
    };

})();
