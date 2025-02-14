import { useState, ChangeEvent } from "react";
import useFetch from "../hooks/useFetch";
import { IUser } from "../interfaces/IUser";

interface AddUserProps {
    onChooseUserChange: (user: { id: string }) => void;
    onCreateUser: (createdUser: { firstName: string; lastName: string, email: string }) => void;
    user: { firstName: string; lastName: string, email: string }
    userId?: number;
}
const AddUser: React.FC<AddUserProps> = ({ onChooseUserChange, onCreateUser, user, userId }) => {
    const [chooseOrCreate, setChooseOrCreate] = useState({
        choose: true,
        create: false
    });
    const { data, loading, error } = useFetch<IUser[]>('api/User');

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        onCreateUser({ ...user, [e.target.name]: e.target.value });
    };

    return (
        <div className="flex flex-col justify-center items-center p-12">
            <h2 className="text-2xl mb-2" >User</h2>
            <div className="flex flex-wrap gap-2 mb-4">
                <button onClick={() => setChooseOrCreate(({ choose: true, create: false }))} className="btn">Choose user</button>
                <button onClick={() => setChooseOrCreate(({ choose: false, create: true }))} className="btn">Create user</button>
            </div>
            {chooseOrCreate.choose &&
                <>
                    {error && <p>{error.toString()}</p>}
                    {loading && <p>loading users...</p>}
                    {data &&
                    <select className="select select-accent w-full max-w-xs"
                        onChange={(e) => onChooseUserChange({ id: e.target.value })}
                        defaultValue={userId || ""}>
                        <option disabled value={userId || ""}>{userId && userId > 0 ? data?.find(x => x.id == userId)?.firstName : 'Select User'}</option>
                            {data && data.map(user => (
                                <option key={user.id} value={user.id}>
                                    {user.firstName}
                                </option>
                            ))}
                        </select>
                        }
                </>
            }
            {chooseOrCreate.create &&
                <>
                    <input
                        value={user.firstName}
                        onChange={handleChange}
                        type="text"
                        name="firstName"
                        placeholder="user firstname"
                    className="input input-bordered input-accent w-full max-w-xs mb-3" />
                    <input
                        value={user.lastName}
                        onChange={handleChange}
                        type="text"
                        name="lastName"
                        placeholder="user lastname"
                    className="input input-bordered input-accent w-full max-w-xs mb-3" />
                    <input
                        value={user.email}
                        onChange={handleChange}
                        type="text"
                        name="email"
                        placeholder="user email"
                    className="input input-bordered input-accent w-full max-w-xs mb-3" />
                </>
            }
        </div>
    )
}

export default AddUser;