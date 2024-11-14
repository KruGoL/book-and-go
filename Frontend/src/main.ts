import { createApp } from 'vue';
import { createPinia } from 'pinia';
import { Quasar } from 'quasar';
import router from './router';
import { setApiUrl } from './model/api';
import 'virtual:windi.css'
import 'virtual:windi-devtools'

// Import icon libraries
import '@quasar/extras/roboto-font/roboto-font.css'
import '@quasar/extras/material-icons/material-icons.css'
import '@quasar/extras/fontawesome-v6/fontawesome-v6.css'

// Import Quasar css
import 'quasar/src/css/index.sass'


import App from './App.vue';
const pinia = createPinia()
let myApp = createApp(App)
const getRuntimeConf = async () => {
  const runtimeConf = await fetch('/config/runtime-config.json');
  return await runtimeConf.json();
};

getRuntimeConf().then((json) => {
  setApiUrl(json.API_URL);

  myApp.use(router)
  myApp.use(Quasar, { plugins: {} })
  myApp.use(pinia)
  myApp.mount('#app');
});
