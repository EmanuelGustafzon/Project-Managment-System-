import { useState, ChangeEvent } from "react";
import useFetch from "../hooks/useFetch";
import { ICustomer } from "../interfaces/ICustomer";

interface CustomerFormProps {
    onChooseCustomerChange: (customer: { id: string }) => void;
    onCreateCustomer: (createdCustomer: { name: string; organisationNumber: string }) => void;
    customer: { name: string, organisationNumber: string }
}
const AddCustomer: React.FC<CustomerFormProps> = ({ onChooseCustomerChange, onCreateCustomer, customer }) => {
    const [chooseOrCreate, setChooseOrCreate] = useState({
        choose: true,
        create: false
    });
    const { data, loading, error } = useFetch<ICustomer[]>('api/Customer');

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        onCreateCustomer({ ...customer, [e.target.name]: e.target.value });
    };

    return (
        <div className="flex flex-col justify-center items-center p-12">
            <h2 className="text-2xl mb-2">Customer</h2>
            <div className="flex flex-wrap gap-2 mb-4">
                <button onClick={() => setChooseOrCreate(({ choose: true, create: false }))} className="btn">Choose customer</button>
                <button onClick={() => setChooseOrCreate(({ choose: false, create: true }))} className="btn">Create customer</button>
            </div>
            {chooseOrCreate.choose &&
                <>
                    {error && <p>{error.toString()}</p>}
                    {loading && <p>loading users...</p>}
                    {data &&
                        <select className="select select-info w-full max-w-xs"
                            onChange={(e) => onChooseCustomerChange({ id: e.target.value })}
                            defaultValue="">
                            <option disabled selected>Select Customer</option>
                            {data && data.map(customer => (
                                <option key={customer.id} value={customer.id}>
                                    {customer.name}
                                </option>
                            ))}
                        </select>
                    }
                </>
            }
            {chooseOrCreate.create &&
                <>
                    <input
                        value={customer.name}
                        onChange={handleChange}
                        type="text"
                        name="name"
                        placeholder="Customer Name"
                        className="input input-bordered input-info w-full max-w-xs mb-3" />
                    <input
                        value={customer.organisationNumber}
                        onChange={handleChange}
                        type="text"
                        name="organisationNumber"
                        placeholder="Organisation number"
                        className="input input-bordered input-info w-full max-w-xs" />
                </>
            }
        </div>
    )
}

export default AddCustomer;