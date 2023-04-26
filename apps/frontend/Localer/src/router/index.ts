import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/translate',
      name: 'translate-home',
      component: () => import('../views/TranslateHomeView.vue')
    },
    {
      path: '/translate/:key',
      name: 'translate',
      component: () => import('../views/TranslateView.vue')
    },
    {
      path: '/translate/:pathMatch(.*)*',
      name: 'not-found',
      component: () => import('../views/NotFoundView.vue')
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/moderation',
      name: 'moderation',
      component: () => import('../views/moderation/ModerationHomeView.vue')
    },
    {
      path: '/moderation/recipes',
      name: 'recipes',
      component: () => import('../views/moderation/RecipesView.vue')
    },
    {
      path: '/moderation/users',
      name: 'users',
      component: () => import('../views/moderation/UsersView.vue')
    },
    {
      path: '/moderation/partners',
      name: 'partners',
      component: () => import('../views/moderation/PartnersView.vue')
    },
    {
      path: '/moderation/adverts',
      name: 'adverts',
      component: () => import('../views/moderation/AdvertsView.vue')
    }
  ]
})

export default router
