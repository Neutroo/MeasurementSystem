import { createStore } from 'vuex'

const store = createStore({
    state: () => ({
        token: null,
        username: null
    }),
    mutations: {
        setToken(state, token) {
            state.token = token;
        },
        setUsername(state, username) {
            state.username = username;
        }
    },
    actions: {},
    getters: {
        getToken(state) {
            return state.token;
        },
        getUsername(state) {
            return state.username;
        },
        isLoggedIn(state) {
            return !!state.token;
        }
    }
})

export default store