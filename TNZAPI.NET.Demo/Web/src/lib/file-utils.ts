// Strips the "data:...;base64," prefix FileReader adds — every wire shape that carries file
// content (Attachment(fileName, fileContent), VoiceFileModel.File, ...) expects raw base64 only.
export function readFileAsBase64(file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.onload = () => {
      const result = reader.result as string
      resolve(result.slice(result.indexOf(',') + 1))
    }
    reader.onerror = () => reject(reader.error)
    reader.readAsDataURL(file)
  })
}