import {ref} from "vue";

export function useNotification() {
    const snackbar = ref({
        show: false,
        message: "",
        color: "success",
        timeout: 3000,
    });

    const showNotification = (message: string, type: "success" | "error" = "success", delay = 3000) => {
        snackbar.value = { show: true, message, color: type, timeout: delay };
    };

    const closeNotification = () => {
        snackbar.value.show = false;
    };

    return { snackbar, showNotification, closeNotification };
}
