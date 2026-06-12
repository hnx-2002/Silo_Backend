declare global {
    interface Window {
        $dialog: {
            info: (options: {
                title?: string;
                content: string;
                positiveText?: string;
                negativeText?: string;
                onPositiveClick?: () => void;
                onNegativeClick?: () => void;
            }) => void;
            success: (options: {
                title?: string;
                content: string;
                positiveText?: string;
                negativeText?: string;
                onPositiveClick?: () => void;
                onNegativeClick?: () => void;
            }) => void;
            warning: (options: {
                title?: string;
                content: string;
                positiveText?: string;
                negativeText?: string;
                onPositiveClick?: () => void;
                onNegativeClick?: () => void;
            }) => void;
            error: (options: {
                title?: string;
                content: string;
                positiveText?: string;
                negativeText?: string;
                onPositiveClick?: () => void;
                onNegativeClick?: () => void;
            }) => void;
            create: (options: {
                title?: string;
                content: string;
                positiveText?: string;
                negativeText?: string;
                onPositiveClick?: () => void;
                onNegativeClick?: () => void;
            }) => void;
        };
    }
}

export {};
