import { defineStore } from 'pinia';
export const useStore = defineStore('main', {
    state: () => {
        return { totalPara: {} };
    },
    actions: {
        save(param) {
            this.totalPara = param;
        },
        load() {
            return this.totalPara;
        },
    },
});

