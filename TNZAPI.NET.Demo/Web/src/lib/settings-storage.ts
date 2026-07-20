// Persists the non-secret connection settings (API URL, SSL Verification, Allow Insecure HTTP)
// to localStorage, gated by an explicit opt-in checkbox. Deliberately excludes the Auth Token —
// that stays in the backend's HttpOnly session cookie, never readable by client-side JS, and
// storing it here would give that protection up.
const REMEMBER_KEY = 'tnzapi-demo-remember-settings'
const STORAGE_KEY = 'tnzapi-demo-settings'

export interface StoredConnectionSettings {
  apiUrl: string
  sslVerificationEnabled: boolean
  allowInsecureHttp: boolean
}

export function isRemembering(): boolean {
  return localStorage.getItem(REMEMBER_KEY) === 'true'
}

export function setRemembering(remember: boolean): void {
  localStorage.setItem(REMEMBER_KEY, String(remember))
  if (!remember) {
    localStorage.removeItem(STORAGE_KEY)
  }
}

export function loadStoredSettings(): StoredConnectionSettings | null {
  const raw = localStorage.getItem(STORAGE_KEY)
  if (!raw) return null
  try {
    return JSON.parse(raw) as StoredConnectionSettings
  } catch {
    return null
  }
}

export function saveSettings(settings: StoredConnectionSettings): void {
  if (!isRemembering()) return
  localStorage.setItem(STORAGE_KEY, JSON.stringify(settings))
}