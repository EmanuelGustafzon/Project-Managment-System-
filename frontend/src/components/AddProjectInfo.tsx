
import { ChangeEvent } from "react";
import useFetch from "../hooks/useFetch";
import { IProjectForm } from "../interfaces/IProjectForm";

interface ProjectInformationProps {
    onProjectInfoChange: (projectForm: { name: string, startTime: string, endTime: string, status: string }) => void;
    projectForm: IProjectForm;
}
const AddProjectInfo: React.FC<ProjectInformationProps> = ({ onProjectInfoChange, projectForm }) => {
    const { data, loading, error } = useFetch<string[]>('api/Project/statuses');

    const handleChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        onProjectInfoChange({ ...projectForm, [e.target.name]: e.target.value });
    };

    return (
        <div className= "flex flex-col justify-center items-center" >
            <h2>Project name </h2>
            <input
                onChange={handleChange}
                value={projectForm.name}
                type="text"
                name="name"
                placeholder = "Name"
                className = "input input-bordered input-info w-full max-w-xs" />
            <h2>Start Date </h2>
            <input
                onChange={handleChange}
                value={projectForm.startTime}
                name="startTime"
                type = "date"
                className = "input input-bordered input-info w-full max-w-xs" />
            <h2>End Date </h2>
            <input
                onChange={handleChange}
                value={projectForm.endTime}
                name="endTime"
                type = "date"
                className = "input input-bordered input-info w-full max-w-xs" />
            <h2>Status </h2>
            { error && <p>{ error.toString() } </p> }
            { loading && <p>loading users...</p> }
            { data &&
                <select className="select select-info w-full max-w-xs"
                    onChange={handleChange}
                    name="status"
                    defaultValue={projectForm.status} >
                    <option disabled value="" > Select Status </option>
                    { data && data.map(status => (
                        <option key= { status } value = { status } >
                        { status }
                        </option>
                        ))
                    }
                </select>
                }
        </div>
    )
}

export default AddProjectInfo;