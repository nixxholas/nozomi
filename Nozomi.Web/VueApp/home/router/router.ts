import VueRouter from 'vue-router'

// Pages
import Characters from '@/home/views/Characters.vue'

const routePrefix = ''

const routes = [
	{
		path: '*',
		component: Characters
	},
	{
		name: 'characters',
		path: `/${routePrefix}`,
		component: Characters
	}
]

export const router = new VueRouter({
	mode: 'history',
	routes,
	linkActiveClass: 'is-active'
})
