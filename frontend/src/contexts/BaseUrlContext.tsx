import { createContext, useContext } from "react";

const BASE_URL = "https://localhost:7172";

const BaseUrlContext = createContext<string>(BASE_URL);

export const useBaseUrl = () => useContext(BaseUrlContext);

export const BaseUrlProvider = ({ children }: { children: React.ReactNode }) => {
    return <BaseUrlContext.Provider value={BASE_URL}>{children}</BaseUrlContext.Provider>;
};
