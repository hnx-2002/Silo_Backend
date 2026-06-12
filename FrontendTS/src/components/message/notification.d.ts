declare global {
    interface Window {
        $notification: {
            info: (options: {
                title?: string;
                content: string;
                duration?: number;
                closable?: boolean;
            }) => void;
            warning: (options: {
                title?: string;
                content: string;
                duration?: number;
                closable?: boolean;
            }) => void;
            error: (options: {
                title?: string;
                content: string;
                duration?: number;
                closable?: boolean;
            }) => void;
            success: (options: {
                title?: string;
                content: string;
                duration?: number;
                closable?: boolean;
            }) => void;
        };
    }
}

export {};
