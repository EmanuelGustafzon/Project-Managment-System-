import { useCallback, useState } from "react";
import { useBaseUrl } from "../contexts/BaseUrlContext";

type ValidationErrorResponse = {
    errors: {
        [field: string]: string[];
    };
};
function useSendData(method: string, endpoint: string, url?: string) {
    const baseUrl = useBaseUrl()
    if (!url) url = baseUrl;

    const [response, setResponse] = useState<string | null>(null)
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>();
    const [validationError, setValidationError] = useState<ValidationErrorResponse>();

    const makeRequest = useCallback(async (data: unknown) => {
        setLoading(true)
        try {
            const res = await fetch(`${url}/${endpoint}`, {
                method: `${method}`,
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(data)
            });
            const resData = await res.json();
            if (resData.errors != null) {
                setValidationError(resData.errors);
                return;
            }
            if (resData.success == false) {
                setError(resData.errorMessage);
                return;
            }
            setError(undefined);
            setValidationError(undefined);
            setResponse(resData.data)
        } catch {
            setError('An error occured')

        } finally {
            setLoading(false)
        }
    }, [method, endpoint, url])
    return {
        makeRequest,
        response,
        loading,
        error,
        validationError
    };
}

export default useSendData;