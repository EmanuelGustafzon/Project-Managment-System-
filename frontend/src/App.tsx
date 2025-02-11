
import './App.css'
import Navbar from './components/Navbar'
import ProjectForm from './components/ProjectForm'
import ProjectList from './components/ProjectList'

function App() {

  return (
      <div className="mt-0">
          <Navbar /> 
          <ProjectList />
          <ProjectForm />
  
    </div>
  )
}

export default App
