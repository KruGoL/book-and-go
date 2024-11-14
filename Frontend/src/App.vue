<template>
  <q-layout view="hHh Lpr lFf">
    <q-header elevated color="grey-9">
      <q-toolbar class="bg-gray-800">
        <q-btn flat @click="drawer = !drawer" round dense icon="menu" />
        <q-toolbar-title> Book-n-Go </q-toolbar-title>
        <router-link to="/profile">
        <q-avatar>
          <img src="https://cdn.quasar.dev/img/avatar.png">
        </q-avatar>
      </router-link>
        <sign-in/>
      </q-toolbar>
    </q-header>
    <q-drawer
      v-model="drawer"
      show-if-above
      :mini="miniState"
      @mouseover="miniState = false"
      @mouseout="miniState = true"
      :width="200"
      :breakpoint="500"
      bordered
      class="left-menu"
    >
      <q-scroll-area class="fit" :horizontal-thumb-style="{ opacity: '0' }">
        <q-list padding>
          <router-link
            class="menu"
            v-for="(route, i) in routes"
            :key="i"
            :to="route.to"
          >
            <q-item class="menu-btn" clickable v-ripple>
              <q-item-section avatar>
                <q-icon :name="route.icon"></q-icon>
              </q-item-section>

              <q-item-section>
                {{ route.title }}
              </q-item-section>
            </q-item>
          </router-link>
        </q-list>
      </q-scroll-area>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import Footer from './components/Footer.vue';
import SignIn from './components/SignIn.vue';

export default defineComponent({
  name: 'App',

  components: {
    'webapp-footer': Footer,
    'sign-in': SignIn,
  },

  data() {
    return {
      routes: [
        {
          to: '/',
          title: 'Home',
          icon: 'fa-solid fa-house',
        },
        {
          to: '/adminPage/',
          title: 'Admin Page',
          icon: 'fa-solid fa-table',
        },
        {
          to: '/editor/',
          title: 'Editor',
          icon: 'fa-solid fa-brush',
        },
      ],
    };
  },
  setup() {
    const leftDrawerOpen = ref(true);
    return {
      leftDrawerOpen,
      drawer: ref(false),
      miniState: ref(true),
    };
  },
  methods: {
    sizeC() {
      const size = this.$refs.myRef;
      console.log(size);
      return size;
    },
  },
});
</script>

<style lang="scss">
.menu {
  text-align: left;

}
.menu-btn {
  margin-top: 20px;
  color: $left-menu-btn;
}
.left-menu {
  background-color: $left-menu;
}
</style>
