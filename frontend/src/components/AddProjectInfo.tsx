import useFetch from "../hooks/useFetch";

interface ProjectInformationProps {
    onStatusChange: (status: string) => void;
}
const AddProjectInfo: React.FC<ProjectInformationProps> = ({ onStatusChange }) => {
    const { data, loading, error } = useFetch<string[]>('api/Project/statuses');
    return (
        <div className= "flex flex-col justify-center items-center" >
            <h2>Project name </h2>
            <input
                type = "text"
                placeholder = "Name"
                className = "input input-bordered input-info w-full max-w-xs" />
            <h2>Start Date </h2>
            <input
                type = "date"
                className = "input input-bordered input-info w-full max-w-xs" />
            <h2>End Date </h2>
            <input
                type = "date"
                className = "input input-bordered input-info w-full max-w-xs" />
            <h2>Status </h2>
            { error && <p>{ error.toString() } </p> }
            { loading && <p>loading users...</p> }
            { data &&
                <select className="select select-info w-full max-w-xs"
                    onChange = {(e) => onStatusChange(e.target.value)}
                    defaultValue = "" >
                    <option disabled selected > Select Status </option>
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