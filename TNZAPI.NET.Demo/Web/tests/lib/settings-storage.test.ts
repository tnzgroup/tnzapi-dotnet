import { describe, it, expect, beforeEach } from 'vitest'
import {
  isRemembering,
  setRemembering,
  loadStoredSettings,
  saveSettings,
  type StoredConnectionSettings,
} from '@/lib/settings-storage'

const SAMPLE: StoredConnectionSettings = {
  apiUrl: 'https://api.tnz.co.nz',
  sslVerificationEnabled: true,
  allowInsecureHttp: false,
}

describe('settings-storage', () => {
  beforeEach(() => {
    localStorage.clear()
  })

  it('defaults to not remembering', () => {
    expect(isRemembering()).toBe(false)
  })

  it('setRemembering(true) persists the flag', () => {
    setRemembering(true)

    expect(isRemembering()).toBe(true)
  })

  it('setRemembering(false) clears any previously stored settings', () => {
    setRemembering(true)
    saveSettings(SAMPLE)
    expect(loadStoredSettings()).toEqual(SAMPLE)

    setRemembering(false)

    expect(isRemembering()).toBe(false)
    expect(loadStoredSettings()).toBeNull()
  })

  it('loadStoredSettings returns null when nothing has been saved', () => {
    expect(loadStoredSettings()).toBeNull()
  })

  it('loadStoredSettings returns null instead of throwing on corrupt JSON', () => {
    localStorage.setItem('tnzapi-demo-settings', 'not json{')

    expect(loadStoredSettings()).toBeNull()
  })

  it('saveSettings is a no-op when not remembering — Save must not silently start persisting', () => {
    saveSettings(SAMPLE)

    expect(loadStoredSettings()).toBeNull()
  })

  it('saveSettings persists when remembering is on, and round-trips the exact value', () => {
    setRemembering(true)

    saveSettings(SAMPLE)

    expect(loadStoredSettings()).toEqual(SAMPLE)
  })

  it('the Auth Token is never part of the persisted shape', () => {
    // StoredConnectionSettings has no token field at the type level — this pins that at runtime
    // too, since it's a deliberate security property (see the file's own header comment) that a
    // future field addition could accidentally violate.
    setRemembering(true)
    saveSettings(SAMPLE)

    const raw = localStorage.getItem('tnzapi-demo-settings')
    expect(raw).not.toBeNull()
    expect(JSON.parse(raw!)).not.toHaveProperty('token')
    expect(JSON.parse(raw!)).not.toHaveProperty('authToken')
  })
})