import { defineStore } from 'pinia';

interface TotalPara {
    [key: string]: any;
}

interface TotalState {
    totalPara: TotalPara;
}

export const useStore = defineStore('main', {
    state: (): TotalState => {
        return { totalPara: {} };
    },
    actions: {
        /**
         * 保存参数到 totalPara
         * @param param - 要保存的参数
         */
        save(param: TotalPara): void {
            this.totalPara = param;
        },
        /**
         * 加载 totalPara
         * @returns totalPara 对象
         */
        load(): TotalPara {
            return this.totalPara;
        },
    },
});
