import { useState, useEffect } from "react";

function useFetch<T = unknown>(endpoint: string, url?: string) {
    const baseUrl = 'https://localhost:7172';
    if (!url) url = baseUrl;

    const [data, setData] = useState<T | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<Error | null>(null);

    useEffect(() => {
        const getData = async () => {
            try {
                const response = await fetch(`${url}/${endpoint}`);
                const result = await response.json();
                console.log(result)
                if (result.ErrorMessage) {
                    setError(result.ErrorMessage);
                    return;
                }
                setData(result.data);
            } catch (error) {
                if (error instanceof Error) {
                    setError(error);
                } else {
                    setError(new Error("An error occured, please try again."));
                }

            } finally {
                setLoading(false)
            }
        };

        getData();

    }, [url, endpoint]);

    return {
        data,
        loading,
        error
    };
}

export default useFetch;