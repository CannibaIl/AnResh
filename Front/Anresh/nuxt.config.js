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
    '~/assets/css/index.scss'
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
    'bootstrap-vue/nuxt',
  ],
  axios: {
    accept: '/',
    baseURL: 'https://localhost:5001',
    headers: {
      'Content-Type': 'application/json'
    },
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
