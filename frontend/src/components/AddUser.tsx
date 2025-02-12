import React from "react";
import useFetch from "../hooks/useFetch";
import { IUser } from "../interfaces/IUser";

interface AddUserProps {
    onUserChange: (user: { id: string }) => void;
}
const AddUser: React.FC<AddUserProps> = ({ onUserChange }) => {
    const { data, loading, error } = useFetch<IUser[]>('api/User');

    return (
        <div className="flex flex-col justify-center items-center">
            <h2>User</h2>
            {error && <p>{error.toString()}</p>}
            {loading && <p>loading users...</p>}
            {data && <select className="select select-info w-full max-w-xs"
                onChange={(e) => onUserChange({ id: e.target.value })}
                defaultValue="">
                <option disabled value="">Select User</option>
                {data && data.map(user => (
                    <option key={user.id} value={user.id}>
                        {user.firstName}
                    </option>
                ))}
            </select>}
        </div>
    )
}
export default AddUser;