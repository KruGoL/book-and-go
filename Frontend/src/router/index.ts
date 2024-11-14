import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import HomeVue from '@/views/Home.vue';
import AdminPage from '@/views/AdminPage.vue';
import EditorPage from '@/views/EditorPage.vue'
import ClientPage from '@/views/ClientPage.vue'
import ThanksPage from '@/views/ThanksPage.vue'
import UserProfile from '@/views/UserProfile.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: HomeVue,

  },
  {
    path: '/adminPage/',
    name: 'Admin Page',
    component: AdminPage,
  },
  {
    path: '/editor/',
    name: 'Admin Editor',
    component: EditorPage,
  },
  {
    path: '/client/:id',
    name: 'Client Page',
    component: ClientPage,
  },
  {
    path: '/Thanks/:id',
    name: 'Thanks Page',
    component: ThanksPage,
  },
  {
    path: '/profile',
    name: 'User Profile',
    component: UserProfile,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
