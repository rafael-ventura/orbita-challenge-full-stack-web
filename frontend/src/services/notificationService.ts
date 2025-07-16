export class NotificationService {
  private static snackbar = {
    show: false,
    text: '',
    color: 'success'
  }

  static setSnackbar(snackbar: any): void {
    this.snackbar = snackbar
  }

  static success(message: string): void {
    this.snackbar.show = true
    this.snackbar.text = message
    this.snackbar.color = 'success'
  }

  static error(message: string): void {
    this.snackbar.show = true
    this.snackbar.text = message
    this.snackbar.color = 'error'
  }

  static warning(message: string): void {
    this.snackbar.show = true
    this.snackbar.text = message
    this.snackbar.color = 'warning'
  }

  static info(message: string): void {
    this.snackbar.show = true
    this.snackbar.text = message
    this.snackbar.color = 'info'
  }

  static showError(error: any): void {
    let message = 'Ocorreu um erro inesperado'
    
    if (error.response?.data?.message) {
      message = error.response.data.message
    } else if (error.message) {
      message = error.message
    }
    
    this.error(message)
  }

  static getSnackbar() {
    return this.snackbar
  }
} 