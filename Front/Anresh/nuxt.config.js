const pkg = require('./package')


module.exports = {
  mode: 'universal',

  /*
  ** Headers of the page
  */
  head: {
    title: 'Anresh',
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: pkg.description }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }
    ]
  },

  /*
  ** Customize the progress-bar color
  */
  loading: { color: '#ee2b45'},

  /*
  ** Global CSS
  */
  css: [
    '~/assets/css/index.scss',
    '~/assets/css/auth.scss'
  ],

  /*
  ** Plugins to load before mounting the App
  */
  plugins: [
    {src:'~/plugins/vue-notification.js', ssr: false},
    "~/plugins/getFullName.js"
  ],

  /*
  ** Nuxt.js modules
  */
  buildModules: [
    '@nuxtjs/fontawesome',
  ],
  modules: [
    '@nuxtjs/axios',
    '@nuxtjs/auth-next',
    'bootstrap-vue/nuxt',
  ],
  axios: {
    accept: '/',
    baseURL: 'http://localhost:5000',
    headers: {
      'Content-Type': 'application/json'
    },
  },
  auth: {
    redirect: {
      login: '/login',
      logout: '/login',
    },
    localStorage: false,
    cookie: {
      options: {
        expires: 7
      }
    },
    strategies: {
      local: {
        token: {
          property: 'token',
          global: true,
          required: true,
          type: 'Bearer'
        },
        user: {
          property: false 
        },
        endpoints: {
          login: {
            url: '/api/user/authenticate',
            method: 'post',
          },
          logout: false,
          refreshToken: false,
          user: { 
            url: '/api/user', 
            method: 'get',
          }
        }
      }
    }
  },
  fontawesome: {
    component: 'fa',
    icons: {
      solid: true,
      brands: true
    }
  },
  


  /*
  ** Build configuration
  */
  build: {
    /*
    ** You can extend webpack config here
    */
    loaders: {
      sass: {
        implementation: require('sass'),
      },
      scss: {
        implementation: require('sass'),
      },
    },
    extend(config, ctx) {

    }
  }
}
