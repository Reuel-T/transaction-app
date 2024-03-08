import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '@/views/HomeView.vue';
import NotFound from '@/views/NotFound.vue';
import ClientView from '@/views/ClientView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/client/:id',
      name: 'clientView',
      component: ClientView
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'notfound',
      component: NotFound
    },
  ]
})

export default router
