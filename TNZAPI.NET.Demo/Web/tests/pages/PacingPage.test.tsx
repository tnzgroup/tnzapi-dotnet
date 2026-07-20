import { describe, it, expect, vi, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import PacingPage from '@/pages/actions/PacingPage'
import { apiRequest } from '@/lib/api-client'

vi.mock('@/lib/api-client', async () => {
  const actual = await vi.importActual<typeof import('@/lib/api-client')>('@/lib/api-client')
  return { ...actual, apiRequest: vi.fn() }
})

const mockedApiRequest = vi.mocked(apiRequest)

describe('PacingPage', () => {
  afterEach(() => {
    mockedApiRequest.mockReset()
  })

  it('submits Message ID, defaults Channel to TTS, and converts Number Of Operators to a number', async () => {
    mockedApiRequest.mockResolvedValue({ status: 200, data: { ActionResult: 'OK' } })
    const user = userEvent.setup()
    render(<PacingPage />)

    await user.type(screen.getByLabelText('Message ID'), 'abc-123')
    await user.type(screen.getByLabelText('Number Of Operators'), '5')
    await user.click(screen.getByRole('button', { name: 'Update Pacing' }))

    expect(mockedApiRequest).toHaveBeenCalledWith(
      '/api/actions/pacing',
      expect.objectContaining({
        method: 'POST',
        body: { MessageID: 'abc-123', Channel: 'TTS', NumberOfOperators: 5 },
      }),
    )
  })

  it('defaults Number Of Operators to 0 when left blank', async () => {
    mockedApiRequest.mockResolvedValue({ status: 200, data: {} })
    const user = userEvent.setup()
    render(<PacingPage />)

    await user.type(screen.getByLabelText('Message ID'), 'abc-123')
    await user.click(screen.getByRole('button', { name: 'Update Pacing' }))

    const body = mockedApiRequest.mock.calls[0][1]?.body as { NumberOfOperators: number }
    expect(body.NumberOfOperators).toBe(0)
  })

  it('only offers TTS and Voice', async () => {
    const user = userEvent.setup()
    render(<PacingPage />)

    await user.click(screen.getByLabelText('Channel'))

    expect(await screen.findByRole('option', { name: 'TTS' })).toBeInTheDocument()
    expect(screen.getByRole('option', { name: 'Voice' })).toBeInTheDocument()
    expect(screen.queryByRole('option', { name: 'SMS' })).not.toBeInTheDocument()
  })

  it('renders the response', async () => {
    mockedApiRequest.mockResolvedValue({ status: 200, data: { ActionResult: 'OK' } })
    const user = userEvent.setup()
    render(<PacingPage />)

    await user.type(screen.getByLabelText('Message ID'), 'abc-123')
    await user.type(screen.getByLabelText('Number Of Operators'), '5')
    await user.click(screen.getByRole('button', { name: 'Update Pacing' }))

    expect(await screen.findByText('Success (HTTP 200)')).toBeInTheDocument()
  })
})