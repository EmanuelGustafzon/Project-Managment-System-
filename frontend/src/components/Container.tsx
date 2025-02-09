import React, { ReactNode } from "react";

interface IContainerProps {
    children: ReactNode;
}

const Container: React.FC<IContainerProps> = ({ children } ) => {
    return (
        <div className="max-w-[720px] m-auto w-full">
            {children}
        </div>




    )
}

export default Container;