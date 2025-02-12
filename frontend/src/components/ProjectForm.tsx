import { useState } from "react";
import AddUser from "./AddUser";
import AddCustomer from "./AddCustomer";
import AddProjectInfo from "./AddProjectInfo";
import { IProjectForm } from "../interfaces/IProjectForm";
import AddService from "./AddService";
import useSendData from "../hooks/useSendData";


const ProjectForm = () => {
    const { makeRequest, validationError, error, response } = useSendData("api/Project")

    const [projectForm, setProjectForm] = useState<IProjectForm>({
        name: "",
        startTime: "",
        endTime: "",
        status: "",
        projectManagerId: 0,
        serviceId: 1,
        customerId: 0,
        customerForm:
        {
            name: "",
            organisationNumber: ""
        }
    });

    
    // form steps
    const totalSteps = 5;
    const [step, setStep] = useState(0);
    // step 1 add info about project
    const handleProjectInfoChange = (projectForm : { name: string, startTime: string, endTime: string, status: string }) => {
        setProjectForm(prev => ({
            ...prev,
            name: projectForm.name,
            startTime: projectForm.startTime,
            endTime: projectForm.endTime,
            status: projectForm.status,
        }));
    };
    // stetp 2 add user
    const handleSelectUserChange = (user: { id: string }) => {
        setProjectForm(prev => ({
            ...prev,
            projectManagerId: Number(user.id)
        }));
    };
    // step 3 add customer
    const handleSelectCustomerChange = (customer: { id: string }) => {
        setProjectForm(prev => ({
            ...prev,
            customerId: Number(customer.id),
            customerForm: {
                name: "", organisationNumber : ""
            }
        }));
    };
    const handleCreateCustomerChange = (createdCustomer: { name: string; organisationNumber: string }) => {
        setProjectForm(prev => ({
            ...prev,
            customerForm: createdCustomer,
            customerId: 0
        }));
        console.log(projectForm)
    }
    // step 4 add service
    //
    // step 5 create project action
    const createProject = () => {
        if (projectForm.customerId > 0) projectForm.customerForm = null;
        makeRequest(projectForm);
    }

    return (
        <div className="bg-sky-100 p-3">
            {step === 0 && <AddProjectInfo onProjectInfoChange={handleProjectInfoChange} projectForm={projectForm} />}
            {step === 1 && <AddUser onUserChange={handleSelectUserChange} />}
            {step === 2 && <AddCustomer onChooseCustomerChange={handleSelectCustomerChange} onCreateCustomer={handleCreateCustomerChange} customer={projectForm.customerForm!} customerId={projectForm.customerId} />}
            {step === 3 && <AddService />}
            {step === 4 &&
                <div>
                    <button className="btn" onClick={createProject}>submit</button>
                    <ul>
                        {validationError &&
                            <div>
                                <h2 className="font-extrabold text-lg">Sorry there was some errors please recheck the form</h2>
                                {Object.entries(validationError).map(([key, value]) => (
                                    <li>
                                        <p className="font-bold"> Field: {key}</p>
                                        <p className="text-red-400"> Error: {value[0]}</p>
                                    </li>
                                ))}
                            </div>
                        }
                    </ul>
                    {error && <p className="text-red-400">{error}</p>}
                    {error && <p className="text-red-400">{error}</p>}
                    {response && <p>{response}</p> }
                </div>}
            <div className="flex flex-wrap justify-center items-center gap-2 m-5">
                <button className="btn" onClick={() => setStep(lastStep => lastStep - 1)}>Prev</button>
                {step <= totalSteps }
                <button className="btn"  onClick={() => setStep(lastStep => lastStep + 1)}>next</button>
            </div>
            
        </div>
    )
}

export default ProjectForm;