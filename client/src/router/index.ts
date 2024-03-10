import { createRouter, createWebHistory } from 'vue-router'
import ClientListView from '@/views/ClientListView.vue'
import NotFound from '@/views/NotFound.vue'
import ClientView from '@/views/ClientView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: ClientListView,
      meta: { transition: 'route-back' }
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
    }
  ]
})

export default router
