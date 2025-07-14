export function useAuthApi() {
    const config = useRuntimeConfig();
    const apiBaseUrl = config.public.apiBaseUrl;
    const isAuthenticated = useState("isAuthenticated", () => false);
    const login = async (credentials: { email: string; password: string }) => {
        try {
            const response = await $fetch<{ id: number; name: string; email: string; token: string }>(
                `${apiBaseUrl}/auth/login`,
                {
                    method: "POST",
                    body: credentials,
                }
            );

            console.log("response:", response);
            localStorage.setItem("token", response.token);
            localStorage.setItem("userId", response.id.toString());
            localStorage.setItem("userName", response.name);
            localStorage.setItem("userEmail", response.email);

            isAuthenticated.value = true;
            return response;
        } catch (err) {
            console.error(" Erro ao fazer login:", err);
            throw err;
        }
    };

    const register = async (userData: { name: string; email: string; password: string }) => {
        try {
            return await $fetch(`${apiBaseUrl}/auth/register`, {
                method: "POST",
                body: userData,
            });
        } catch (err) {
            console.error(" Erro ao registrar usuário:", err);
            throw err;
        }
    };

    const getUser = async () => {
        try {
            return await $fetch(`${apiBaseUrl}/auth/user`, {method: "GET"});
        } catch (err) {
            console.error(" Erro ao buscar usuário:", err);
            throw err;
        }
    };

    return {login, register, getUser};
}
