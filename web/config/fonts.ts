import localFont from 'next/font/local'

export const supreme = localFont({
  src: [
    {
      path: '../fonts/Supreme-Extralight.otf',
      weight: '200',
      style: 'normal',
    },
    {
      path: '../fonts/Supreme-Light.otf',
      weight: '300',
      style: 'normal',
    },
    {
      path: '../fonts/Supreme-Regular.otf',
      weight: '400',
      style: 'normal',
    },
    {
      path: '../fonts/Supreme-Medium.otf',
      weight: '500',
      style: 'normal',
    },
    {
      path: '../fonts/Supreme-Bold.otf',
      weight: '700',
      style: 'normal',
    },
  ],
})
