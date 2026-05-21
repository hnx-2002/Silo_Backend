declare global {
    interface Window {
        $message: {
            info: (content: string) => void;
            warning: (content: string) => void;
            error: (content: string) => void;
            success: (content: string) => void;
        };
    }
}

export {};
