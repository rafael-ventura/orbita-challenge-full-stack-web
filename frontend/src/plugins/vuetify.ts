import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import { aliases, mdi } from 'vuetify/iconsets/mdi'
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

// Importar estilos customizados
import '@/assets/styles/app.scss'

export default createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'light',
    themes: {
      light: {
        colors: {
          primary: '#c8102e',
          secondary: '#e63946',
          accent: '#ff8a80',
          error: '#b71c1c',
          warning: '#ff9800',
          info: '#2196f3',
          success: '#4caf50'
        }
      }
    }
  },
  icons: {
    defaultSet: 'mdi',
    aliases,
    sets: {
      mdi
    }
  },
  defaults: {
    VBtn: {
      color: 'primary',
      variant: 'outlined'
    },
    VCard: {
      elevation: 2
    },
    VContainer: {
      fluid: true
    }
  },
  display: {
    mobileBreakpoint: 'sm',
    thresholds: {
      xs: 0,
      sm: 340,
      md: 540,
      lg: 800,
      xl: 1280,
    },
  }
}) 