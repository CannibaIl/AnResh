import Vue from 'vue'
import Notifications from 'vue-notification'
import velocity from 'velocity-animate'

Vue.use(Notifications, { velocity })

function notify(options) {
  Vue.notify({
    type: options.type,
    title: options.title,
    text: options.text,
    closeOnClick: true,
    duration: options.type === 'error' ? 30000 : 10000
  })
}

function notifyInfo(title, text) {
  notify({
    type: 'success',
    title: title,
    text: text,
  })
}
Vue.prototype.$notifyInfo = notifyInfo

function notifyError(title, text) {
  notify({
    type: 'error',
    title: title,
    text: text,
  })
}
Vue.prototype.$notifyError = notifyError

function notifyWarn(title, text) {
  notify({
    type: 'warn',
    title: title,
    text: text,
  })
}
Vue.prototype.$notifyWarn = notifyWarn
