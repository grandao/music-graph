local prefix = 'KEPSREC'
local names = {'C', 'Db', 'D', 'Eb', 'E', 'F', 'Gb', 'G', 'Ab', 'A', 'Bb', 'B'}


for i = 21, 108 do
    local idx = (i % 12) + 1
    local oct = math.floor(i / 12) - 1
    local inp = string.format('%s%03d.wav', prefix, i)
    local out = string.format('%s%d.wav', names[idx], oct)

    os.execute(string.format('ffmpeg.exe -i %s -ss 0 -t 15  %s', inp, out))
end