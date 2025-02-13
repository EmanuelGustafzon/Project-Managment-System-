import { useState, ChangeEvent } from "react";
import useFetch from "../hooks/useFetch";
import { ICurrancy } from "../interfaces/ICurrancy";
import { IService } from "../interfaces/IService";

interface serviceFormProps {
    onChooseServiceChange: (service: { id: string }) => void;
    onCreateService: (createdService: {
        name: string,
        currency: string,
        unit: string,
        price: number,
    }) => void;
    service: { name: string, currency: string, unit: string, price: number }
    serviceId?: number;
}

const AddService: React.FC<serviceFormProps> = ({ onChooseServiceChange, onCreateService, service, serviceId }) => {

    const [chooseOrCreate, setChooseOrCreate] = useState({
        choose: true,
        create: false
    });

    const { data, loading, error } = useFetch<IService[]>('api/Service');
    const units = useFetch<string[]>('api/Service/units');
    const customerData = useFetch<ICurrancy[]>('api/Currency');
    console.log(units)

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        onCreateService({ ...service, [e.target.name]: e.target.value });
    };

    return (
        <div className="flex flex-col justify-center items-center p-12">
            <h2 className="text-2xl mb-2">Service</h2>
            <div className="flex flex-wrap gap-2 mb-4">
                <button onClick={() => setChooseOrCreate(({ choose: true, create: false }))} className="btn">Choose service</button>
                <button onClick={() => setChooseOrCreate(({ choose: false, create: true }))} className="btn">Create service</button>
            </div>
            {chooseOrCreate.choose &&
                <>
                    {error && <p>{error.toString()}</p>}
                    {loading && <p>loading services...</p>}
                    {data &&
                    <select className="select select-info w-full max-w-xs"
                        onChange={(e) => onChooseServiceChange({ id: e.target.value })}
                        defaultValue={serviceId || ""}>
                        <option disabled value={serviceId || ""}> {serviceId && serviceId > 0 ? data?.find(x => x.id == serviceId)?.name : 'Select Service'} </option>
                            {data && data.map(service => (
                                <option key={service.id} value={service.id}>
                                    {service.name}
                                </option>
                            ))}
                        </select>
                    }
                </>
            }
            {chooseOrCreate.create &&
                <>
                    <input
                        value={service.name}
                        onChange={handleChange}
                        type="text"
                        name="name"
                        placeholder="Service Name"
                        className="input input-bordered input-info w-full max-w-xs mb-3" />
                    <label className="w-full max-w-xs">
                        <p>Choose a currancy or type your own:</p>
                        <input
                            value={service.currency}
                            type="text"
                            list="currancies"
                            name="currancy"
                            onChange={handleChange}
                            className="input input-bordered input-info w-full max-w-xs mb-2" />
                                {customerData.loading && <p>loading currencies...</p>}
                                    <datalist id="currancies">
                                        {customerData.data &&
                                        customerData.data.map((currency) => (
                                        <option key={currency.id} value={currency.currency} />
                                        ))}
                                    </datalist>
                    </label>  
                <label className="w-full max-w-xs mb-2">
                    <p>Service Price</p>
                        <input
                            value={service.price > 0 ? service.price : ""}
                            onChange={handleChange}
                            type="number"
                            name="price"
                            placeholder="ex: 200"
                            className="input input-bordered input-info w-full max-w-xs" />
                    </label>
                    

                    {units.error && <p>{units.error.toString()}</p>}
                    {units.loading && <p>loading units...</p>}
                    {units.data &&
                        <select className="select select-info w-full max-w-xs"
                            onChange={handleChange}
                            name="unit"
                            defaultValue={service.unit || ""}>
                                <option disabled value={service.unit || ""}> {service.unit ? service.unit : "select unit"}</option>
                                {units.data && units.data.map(unit=> (
                                    <option key={unit} value={unit}>
                                        {unit}
                                    </option>
                                ))}
                        </select>
                    }
                </>
            }
        </div>
    )
}

export default AddService;