import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import Container from './components/Container.tsx'
import { BaseUrlProvider } from './contexts/BaseUrlContext.tsx'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BaseUrlProvider>
            <Container>
                <App />
            </Container>
        </BaseUrlProvider>
  </StrictMode>,
)
